using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services;

// Pure in-memory holder of the tokens, registered as a Singleton so every TokenStore instance
// shares it (see TokenStore's comment).
public sealed class TokenCache
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public bool Loaded { get; set; }
    public SemaphoreSlim Gate { get; } = new(1, 1);
}

// Centralizes the token storage keys so the auth state provider and the bearer-attaching handler
// never drift out of sync with each other. The tokens are kept in per-tab session storage
// (SessionTokenStorage), so closing the tab or browser signs the user out.
//
// TokenStore is Scoped, but the token values live in the TokenCache singleton: IHttpClientFactory
// resolves message handlers (AuthorizationMessageHandler) from its own DI scope, so the handler's
// TokenStore is a different instance from the one components inject, and caching values on
// TokenStore itself wouldn't reach it. (The mobile apps are Flutter, in src/Mobile.)
public class TokenStore(ITokenStorage storage, TokenCache cache)
{
    private const string AccessTokenKey = "landadoc_access_token";
    private const string RefreshTokenKey = "landadoc_refresh_token";

    public async Task SaveAsync(AuthResponse response)
    {
        await storage.SetAsync(AccessTokenKey, response.Token);
        await storage.SetAsync(RefreshTokenKey, response.RefreshToken);
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
        await storage.RemoveAsync(AccessTokenKey);
        await storage.RemoveAsync(RefreshTokenKey);
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
            cache.AccessToken = await storage.GetAsync(AccessTokenKey);
            cache.RefreshToken = await storage.GetAsync(RefreshTokenKey);
            cache.Loaded = true;
        }
        finally
        {
            cache.Gate.Release();
        }
    }
}
