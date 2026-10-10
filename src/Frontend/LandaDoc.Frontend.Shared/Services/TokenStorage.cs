using Blazored.LocalStorage;
using Microsoft.JSInterop;

namespace LandaDoc.Frontend.Shared.Services;

// Where TokenStore keeps the signed-in user's tokens between page loads.
public interface ITokenStorage
{
    Task<string?> GetAsync(string key);
    Task SetAsync(string key, string value);
    Task RemoveAsync(string key);
}

// Long-lived storage: the user stays signed in until they log out (or the refresh token
// expires). The mobile apps use this, where there's no tab to close.
public sealed class LocalTokenStorage(ILocalStorageService localStorage) : ITokenStorage
{
    public async Task<string?> GetAsync(string key) => await localStorage.GetItemAsStringAsync(key);
    public async Task SetAsync(string key, string value) => await localStorage.SetItemAsStringAsync(key, value);
    public async Task RemoveAsync(string key) => await localStorage.RemoveItemAsync(key);
}

// Per-tab storage (window.sessionStorage) for the web apps: it survives a page refresh but is
// gone once the tab or browser is closed, so closing it signs the user out — what you'd expect
// of a medical app on a shared computer. Tokens an earlier version left in localStorage are
// removed, so nobody stays signed in from before.
public sealed class SessionTokenStorage(IJSRuntime js) : ITokenStorage
{
    public async Task<string?> GetAsync(string key)
    {
        await js.InvokeVoidAsync("localStorage.removeItem", key);
        return await js.InvokeAsync<string?>("sessionStorage.getItem", key);
    }

    public async Task SetAsync(string key, string value) =>
        await js.InvokeVoidAsync("sessionStorage.setItem", key, value);

    public async Task RemoveAsync(string key)
    {
        await js.InvokeVoidAsync("sessionStorage.removeItem", key);
        await js.InvokeVoidAsync("localStorage.removeItem", key);
    }
}
