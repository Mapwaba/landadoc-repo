using System.Net.Http.Json;
using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public class NotificationApiClient(HttpClient http) : INotificationApiClient
{
    public async Task<List<NotificationDto>> GetMineAsync(bool unreadOnly) =>
        await http.GetFromJsonAsync<List<NotificationDto>>($"api/notifications/mine?unreadOnly={unreadOnly}") ?? [];

    public Task<NotificationPageDto?> GetPageAsync(int page, int pageSize, bool unreadOnly) =>
        http.GetFromJsonAsync<NotificationPageDto>($"api/notifications/mine/page?page={page}&pageSize={pageSize}&unreadOnly={unreadOnly}");

    public async Task<bool> MarkReadAsync(Guid id)
    {
        var resp = await http.PatchAsync($"api/notifications/{id}/read", null);
        return resp.IsSuccessStatusCode;
    }
}
