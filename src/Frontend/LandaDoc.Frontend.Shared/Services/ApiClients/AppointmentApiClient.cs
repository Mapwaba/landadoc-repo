using System.Net.Http.Json;
using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public class AppointmentApiClient(HttpClient http) : IAppointmentApiClient
{
    public Task<HttpResponseMessage> CreateAsync(CreateAppointmentRequest req) =>
        http.PostAsJsonAsync("api/appointments", req);

    public async Task<List<AppointmentDto>> GetMineAsync() =>
        await http.GetFromJsonAsync<List<AppointmentDto>>("api/appointments/mine", JsonDefaults.Options) ?? [];

    public async Task<AppointmentDto?> GetByIdAsync(Guid id)
    {
        var resp = await http.GetAsync($"api/appointments/{id}");
        return resp.IsSuccessStatusCode
            ? await resp.Content.ReadFromJsonAsync<AppointmentDto>(JsonDefaults.Options)
            : null;
    }

    public Task<HttpResponseMessage> CompleteAsync(Guid id) =>
        http.PatchAsync($"api/appointments/{id}/complete", null);

    public Task<HttpResponseMessage> RescheduleAsync(Guid id, RescheduleRequest req) =>
        http.PatchAsJsonAsync($"api/appointments/{id}/reschedule", req);

    public async Task<AppointmentStatsDto?> GetStatsByDoctorAsync() =>
        await http.GetFromJsonAsync<AppointmentStatsDto>("api/appointments/stats/by-doctor", JsonDefaults.Options);
}
