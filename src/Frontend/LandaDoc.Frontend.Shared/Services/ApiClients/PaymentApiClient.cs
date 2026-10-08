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

    public async Task<List<PaymentDto>> GetMineAsDoctorAsync() =>
        await http.GetFromJsonAsync<List<PaymentDto>>("api/payments/doctor/me", JsonDefaults.Options) ?? [];

    public Task<HttpResponseMessage> InitiateAsync(Guid paymentId, InitiatePaymentRequest request) =>
        http.PostAsJsonAsync($"api/payments/{paymentId}/initiate", request, JsonDefaults.Options);

    public async Task<List<InsurerDto>> GetInsurersAsync() =>
        await http.GetFromJsonAsync<List<InsurerDto>>("api/insurers", JsonDefaults.Options) ?? [];

    public Task<HttpResponseMessage> CreateInsurerAsync(SaveInsurerRequest request) =>
        http.PostAsJsonAsync("api/insurers", request, JsonDefaults.Options);

    public Task<HttpResponseMessage> UpdateInsurerAsync(Guid id, SaveInsurerRequest request) =>
        http.PutAsJsonAsync($"api/insurers/{id}", request, JsonDefaults.Options);

    public async Task<List<InsurerDto>> GetInsurersForDoctorAsync(Guid doctorId) =>
        await http.GetFromJsonAsync<List<InsurerDto>>($"api/insurers/doctor/{doctorId}", JsonDefaults.Options) ?? [];

    public async Task<AcceptedInsurersDto> GetMyAcceptedInsurersAsync() =>
        await http.GetFromJsonAsync<AcceptedInsurersDto>("api/insurers/doctor/me", JsonDefaults.Options) ?? new AcceptedInsurersDto(true, []);

    public Task<HttpResponseMessage> SaveMyAcceptedInsurersAsync(AcceptedInsurersDto choice) =>
        http.PutAsJsonAsync("api/insurers/doctor/me", choice, JsonDefaults.Options);

    public async Task<InsuranceClaimDto?> GetClaimByAppointmentAsync(Guid appointmentId)
    {
        var resp = await http.GetAsync($"api/insurance-claims/by-appointment/{appointmentId}");
        return resp.IsSuccessStatusCode
            ? await resp.Content.ReadFromJsonAsync<InsuranceClaimDto>(JsonDefaults.Options)
            : null;
    }

    public async Task<List<InsuranceClaimDto>> GetMyClaimsAsDoctorAsync() =>
        await http.GetFromJsonAsync<List<InsuranceClaimDto>>("api/insurance-claims/doctor/me", JsonDefaults.Options) ?? [];

    public Task<HttpResponseMessage> ApproveClaimAsync(Guid claimId, string authorizationReference) =>
        http.PostAsJsonAsync($"api/insurance-claims/{claimId}/approve", new ClaimActionRequest(authorizationReference), JsonDefaults.Options);

    public Task<HttpResponseMessage> DeclineClaimAsync(Guid claimId, string? reason) =>
        http.PostAsJsonAsync($"api/insurance-claims/{claimId}/decline", new ClaimActionRequest(reason), JsonDefaults.Options);

    public Task<HttpResponseMessage> SettleClaimAsync(Guid claimId, string? insurerReference) =>
        http.PostAsJsonAsync($"api/insurance-claims/{claimId}/settle", new ClaimActionRequest(insurerReference), JsonDefaults.Options);

    public Task<HttpResponseMessage> RejectClaimAsync(Guid claimId, string? reason) =>
        http.PostAsJsonAsync($"api/insurance-claims/{claimId}/reject", new ClaimActionRequest(reason), JsonDefaults.Options);
}
