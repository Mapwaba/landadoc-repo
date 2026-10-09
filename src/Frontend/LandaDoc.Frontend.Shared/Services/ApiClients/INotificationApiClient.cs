using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public interface INotificationApiClient
{
    Task<List<NotificationDto>> GetMineAsync(bool unreadOnly);
    // One page, newest first, with totals (pageSize: 10, 20, 30, 50 or 100)
    Task<NotificationPageDto?> GetPageAsync(int page, int pageSize, bool unreadOnly);
    Task<bool> MarkReadAsync(Guid id);
}
