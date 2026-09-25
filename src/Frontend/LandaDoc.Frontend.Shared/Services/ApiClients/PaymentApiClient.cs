using System.Net.Http.Json;
using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public class PaymentApiClient(HttpClient http) : IPaymentApiClient
{
    public async Task<PaymentDto?> GetByIdAsync(Guid id)
    {
        var resp = await http.GetAsync($"api/payments/{id}");
        return resp.IsSuccessStatusCode
            ? await resp.Content.ReadFromJsonAsync<PaymentDto>(JsonDefaults.Options)
            : null;
    }

    public async Task<PaymentDto?> GetByAppointmentAsync(Guid appointmentId)
    {
        var resp = await http.GetAsync($"api/payments/by-appointment/{appointmentId}");
        return resp.IsSuccessStatusCode
            ? await resp.Content.ReadFromJsonAsync<PaymentDto>(JsonDefaults.Options)
            : null;
    }

    public Task<HttpResponseMessage> InitiateAsync(Guid paymentId, InitiatePaymentRequest request) =>
        http.PostAsJsonAsync($"api/payments/{paymentId}/initiate", request, JsonDefaults.Options);
}
