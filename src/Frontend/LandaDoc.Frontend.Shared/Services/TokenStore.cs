using Blazored.LocalStorage;
using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services;

// Pure in-memory holder, deliberately free of any IJSRuntime/localStorage dependency, registered
// as a Singleton (see TokenStore's own comment for why TokenStore itself can't be the singleton).
public sealed class TokenCache
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public bool Loaded { get; set; }
    public SemaphoreSlim Gate { get; } = new(1, 1);
}

// Centralizes the localStorage keys so the auth state provider and the
// bearer-attaching handler never drift out of sync with each other.
//
// TokenStore is Scoped (so its ILocalStorageService is always a live, WebView-attached one -
// see below), but the actual token values live in the injected TokenCache singleton, shared by
// every TokenStore instance regardless of which DI scope constructed it. This split exists
// because of two separate MAUI Blazor Hybrid failure modes discovered the hard way:
//
// 1. IHttpClientFactory resolves message handlers (AuthorizationMessageHandler) from its own
//    internal DI scope, separate from the scope the rest of the app uses - so the handler's
//    TokenStore is a *different instance* than the one components inject. Caching values on
//    TokenStore itself wouldn't help the handler's copy.
// 2. JS interop (which Blazored.LocalStorage relies on) is thread-affine to the UI dispatcher.
//    AuthorizationMessageHandler runs inside the HttpClient pipeline, which resumes on a
//    thread-pool thread (a ConfigureAwait(false) inside IHttpClientFactory's built-in logging
//    handler drops the dispatcher's SynchronizationContext before our handler ever runs), so a
//    localStorage read triggered from there hangs forever instead of throwing. Making TokenStore
//    itself a singleton does NOT fix this: a singleton's dependencies are constructed once from
//    the root provider, and here the resulting ILocalStorageService ends up bound to no live
//    WebView at all, so any JS interop call through it throws "Cannot invoke JavaScript outside
//    of a WebView context" - worse than the hang. TokenCache has no such dependency, so it's
//    safe to share globally, and every TokenStore's real JS interop only ever runs from a
//    properly-scoped call site (i.e. a component's own lifecycle method).
//
// EnsureLoadedAsync must be awaited once from a component lifecycle method (on the dispatcher)
// before any HttpClient call can reach the handler - see Routes.razor in the mobile hosts.
public class TokenStore(ILocalStorageService localStorage, TokenCache cache)
{
    private const string AccessTokenKey = "landadoc_access_token";
    private const string RefreshTokenKey = "landadoc_refresh_token";

    public async Task SaveAsync(AuthResponse response)
    {
        await localStorage.SetItemAsStringAsync(AccessTokenKey, response.Token);
        await localStorage.SetItemAsStringAsync(RefreshTokenKey, response.RefreshToken);
        cache.AccessToken = response.Token;
        cache.RefreshToken = response.RefreshToken;
        cache.Loaded = true;
    }

    public async ValueTask<string?> GetAccessTokenAsync()
    {
        await EnsureLoadedAsync();
        return cache.AccessToken;
    }

    public async ValueTask<string?> GetRefreshTokenAsync()
    {
        await EnsureLoadedAsync();
        return cache.RefreshToken;
    }

    public async Task ClearAsync()
    {
        await localStorage.RemoveItemAsync(AccessTokenKey);
        await localStorage.RemoveItemAsync(RefreshTokenKey);
        cache.AccessToken = null;
        cache.RefreshToken = null;
        cache.Loaded = true;
    }

    public async Task EnsureLoadedAsync()
    {
        if (cache.Loaded) return;
        await cache.Gate.WaitAsync();
        try
        {
            if (cache.Loaded) return;
            cache.AccessToken = await localStorage.GetItemAsStringAsync(AccessTokenKey);
            cache.RefreshToken = await localStorage.GetItemAsStringAsync(RefreshTokenKey);
            cache.Loaded = true;
        }
        finally
        {
            cache.Gate.Release();
        }
    }
}
