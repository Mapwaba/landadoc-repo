using System.Net.Http.Json;
using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public class AvailabilityApiClient(HttpClient http) : IAvailabilityApiClient
{
    public async Task<List<string>> GetSlotsAsync(Guid doctorId, DateOnly date) =>
        (await GetSlotListAsync(doctorId, date)).Slots;

    public async Task<SlotsResponse> GetSlotListAsync(Guid doctorId, DateOnly date)
    {
        var resp = await http.GetFromJsonAsync<SlotsResponse>(
            $"api/availability/slots?doctorId={doctorId}&date={date:yyyy-MM-dd}");
        return resp ?? new SlotsResponse([]);
    }

    public async Task<List<ScheduleDayDto>> GetMyScheduleAsync() =>
        await http.GetFromJsonAsync<List<ScheduleDayDto>>("api/availability/schedule/mine", JsonDefaults.Options) ?? [];

    public Task<HttpResponseMessage> SetMyScheduleAsync(UpdateScheduleRequest req) =>
        http.PutAsJsonAsync("api/availability/schedule/mine", req);
}
