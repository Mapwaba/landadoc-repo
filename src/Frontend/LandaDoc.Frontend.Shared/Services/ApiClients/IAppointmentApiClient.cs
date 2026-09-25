using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public interface IAppointmentApiClient
{
    Task<HttpResponseMessage> CreateAsync(CreateAppointmentRequest req);
    Task<List<AppointmentDto>> GetMineAsync();
    Task<AppointmentDto?> GetByIdAsync(Guid id);
    Task<HttpResponseMessage> CompleteAsync(Guid id);
    Task<HttpResponseMessage> RescheduleAsync(Guid id, RescheduleRequest req);
    Task<AppointmentStatsDto?> GetStatsByDoctorAsync();
}
