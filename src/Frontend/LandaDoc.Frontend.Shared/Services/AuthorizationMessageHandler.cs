using System.Net.Http.Headers;

namespace LandaDoc.Frontend.Shared.Services;

public class AuthorizationMessageHandler(TokenStore tokenStore, JwtAuthenticationStateProvider authState) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await tokenStore.GetAccessTokenAsync();
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            await authState.MarkUserAsLoggedOut();

        return response;
    }
}
