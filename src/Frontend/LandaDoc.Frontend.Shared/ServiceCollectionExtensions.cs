using Blazored.LocalStorage;
using LandaDoc.Frontend.Shared.Services;
using LandaDoc.Frontend.Shared.Services.ApiClients;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace LandaDoc.Frontend.Shared;

public static class ServiceCollectionExtensions
{
    // Takes IServiceCollection/IConfiguration rather than WebAssemblyHostBuilder so both the
    // Blazor WASM heads (Admin/Doctor/Patient) and the MAUI Blazor Hybrid heads
    // (Patient.Mobile/Doctor.Mobile) can register the exact same services.
    public static void AddLandaDocFrontendShared(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMudServices();
        services.AddBlazoredLocalStorage();

        services.AddScoped<ILanguageService, LanguageService>();

        // TokenCache (the in-memory token cache, no IJSRuntime dependency) is a singleton shared by
        // every scope; TokenStore itself stays Scoped so its ILocalStorageService is always a live,
        // WebView-attached one. See TokenStore's own comment for why this split - not just making
        // TokenStore a singleton - is what MAUI Blazor Hybrid actually needs here.
        services.AddSingleton<TokenCache>();
        services.AddScoped<TokenStore>();
        services.AddScoped<JwtAuthenticationStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(
            sp => sp.GetRequiredService<JwtAuthenticationStateProvider>());
        services.AddAuthorizationCore();
        services.AddTransient<AuthorizationMessageHandler>();
        services.AddScoped<NotificationHubService>();

        var apiBaseUrls = configuration.GetSection("ApiBaseUrls");

        AddApiClient<IIdentityApiClient, IdentityApiClient>(services, apiBaseUrls["Identity"]!);
        AddApiClient<IAppointmentApiClient, AppointmentApiClient>(services, apiBaseUrls["Appointment"]!);
        AddApiClient<IAvailabilityApiClient, AvailabilityApiClient>(services, apiBaseUrls["Availability"]!);
        AddApiClient<IPaymentApiClient, PaymentApiClient>(services, apiBaseUrls["Payment"]!);
        AddApiClient<IAdminApiClient, AdminApiClient>(services, apiBaseUrls["Admin"]!);
        AddApiClient<ISearchApiClient, SearchApiClient>(services, apiBaseUrls["Search"]!);
        AddApiClient<IDocumentApiClient, DocumentApiClient>(services, apiBaseUrls["Document"]!);
        AddApiClient<IReviewApiClient, ReviewApiClient>(services, apiBaseUrls["Review"]!);
        AddApiClient<INotificationApiClient, NotificationApiClient>(services, apiBaseUrls["Notification"]!);
    }

    private static void AddApiClient<TClient, TImplementation>(IServiceCollection services, string baseUrl)
        where TClient : class
        where TImplementation : class, TClient
    {
        services.AddHttpClient<TClient, TImplementation>(c => c.BaseAddress = new Uri(baseUrl))
            .AddHttpMessageHandler<AuthorizationMessageHandler>();
    }
}
