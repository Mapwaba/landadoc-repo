using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public interface INotificationApiClient
{
    Task<List<NotificationDto>> GetMineAsync(bool unreadOnly);
    Task<bool> MarkReadAsync(Guid id);
}
