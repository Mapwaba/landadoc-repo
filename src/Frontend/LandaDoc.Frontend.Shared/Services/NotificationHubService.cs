using LandaDoc.Shared.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LandaDoc.Frontend.Shared.Services;

// Single real-time channel for both the pending-appointment count badge and live in-app
// notifications (booking received, reminders, payment receipts/failures). Connection lifecycle
// tracks JwtAuthenticationStateProvider so it (re)connects on login and disconnects on logout.
public class NotificationHubService : IAsyncDisposable
{
    private readonly TokenStore tokenStore;
    private readonly AuthenticationStateProvider authState;
    private readonly ILogger<NotificationHubService> logger;
    private readonly string hubUrl;
    private HubConnection? connection;

    public int PendingCount { get; private set; }
    public event Action<int>? PendingCountChanged;
    public event Action<NotificationDto>? NotificationReceived;

    public NotificationHubService(
        TokenStore tokenStore,
        AuthenticationStateProvider authState,
        IConfiguration config,
        ILogger<NotificationHubService> logger)
    {
        this.tokenStore = tokenStore;
        this.authState = authState;
        this.logger = logger;
        hubUrl = $"{config.GetSection("ApiBaseUrls")["Notification"]!.TrimEnd('/')}/hubs/notifications";
        authState.AuthenticationStateChanged += OnAuthenticationStateChanged;
    }

    public async Task InitializeAsync()
    {
        var state = await authState.GetAuthenticationStateAsync();
        if (state.User.Identity?.IsAuthenticated == true)
            await StartAsync();
    }

    private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        var state = await task;
        if (state.User.Identity?.IsAuthenticated == true)
            await StartAsync();
        else
            await StopAsync();
    }

    private async Task StartAsync()
    {
        if (connection is not null) return;

        var newConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl, o => o.AccessTokenProvider = () => tokenStore.GetAccessTokenAsync().AsTask())
            .WithAutomaticReconnect()
            .Build();

        newConnection.On<int>("PendingCountChanged", count =>
        {
            PendingCount = count;
            PendingCountChanged?.Invoke(count);
        });
        newConnection.On<NotificationDto>("NotificationReceived", dto => NotificationReceived?.Invoke(dto));
        newConnection.Closed += ex =>
        {
            logger.LogWarning(ex, "Notification hub connection closed ({HubUrl})", hubUrl);
            return Task.CompletedTask;
        };

        try
        {
            await newConnection.StartAsync();
            logger.LogInformation("Notification hub connected ({HubUrl})", hubUrl);
            connection = newConnection;
        }
        catch (Exception ex)
        {
            // Don't leave `connection` assigned on failure — the guard above would otherwise
            // permanently skip every future retry for the rest of this browser session.
            logger.LogError(ex, "Notification hub failed to connect ({HubUrl})", hubUrl);
            await newConnection.DisposeAsync();
        }
    }

    private async Task StopAsync()
    {
        if (connection is null) return;
        await connection.DisposeAsync();
        connection = null;
        PendingCount = 0;
        PendingCountChanged?.Invoke(0);
    }

    public async ValueTask DisposeAsync()
    {
        authState.AuthenticationStateChanged -= OnAuthenticationStateChanged;
        if (connection is not null) await connection.DisposeAsync();
    }
}
