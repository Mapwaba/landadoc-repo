using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public interface IAvailabilityApiClient
{
    Task<List<string>> GetSlotsAsync(Guid doctorId, DateOnly date);
    Task<List<ScheduleDayDto>> GetMyScheduleAsync();
    Task<HttpResponseMessage> SetMyScheduleAsync(UpdateScheduleRequest req);
}
