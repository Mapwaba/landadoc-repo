using System.Security.Claims;
using LandaDoc.Shared.DTOs;
using Microsoft.AspNetCore.Components.Authorization;

namespace LandaDoc.Frontend.Shared.Services;

public class JwtAuthenticationStateProvider(TokenStore tokenStore) : AuthenticationStateProvider
{
    private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await tokenStore.GetAccessTokenAsync();
        if (string.IsNullOrWhiteSpace(token)) return new AuthenticationState(Anonymous);

        var expiry = JwtParser.GetExpiry(token);
        if (expiry is null || expiry <= DateTimeOffset.UtcNow)
        {
            await tokenStore.ClearAsync();
            return new AuthenticationState(Anonymous);
        }

        var identity = new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task MarkUserAsAuthenticated(AuthResponse response)
    {
        await tokenStore.SaveAsync(response);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    // Same login, changed details (e.g. a new name): lets listeners such as the sidebar reload them
    public void NotifyUserChanged() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    public async Task MarkUserAsLoggedOut()
    {
        await tokenStore.ClearAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(Anonymous)));
    }
}
