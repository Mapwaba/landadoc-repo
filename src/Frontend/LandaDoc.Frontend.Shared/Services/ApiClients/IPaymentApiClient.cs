using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public interface IPaymentApiClient
{
    Task<PaymentDto?> GetByIdAsync(Guid id);
    Task<PaymentDto?> GetByAppointmentAsync(Guid appointmentId);
    Task<List<PaymentDto>> GetMineAsDoctorAsync();
    Task<HttpResponseMessage> InitiateAsync(Guid paymentId, InitiatePaymentRequest request);

    // Insurance / medical aid partners (admins see inactive ones too)
    Task<List<InsurerDto>> GetInsurersAsync();
    Task<HttpResponseMessage> CreateInsurerAsync(SaveInsurerRequest request);
    Task<HttpResponseMessage> UpdateInsurerAsync(Guid id, SaveInsurerRequest request);

    // Insurance claims. The patient files one through InitiateAsync (Provider = Insurance);
    // the doctor then approves/declines it and later records whether the insurer paid.
    Task<InsuranceClaimDto?> GetClaimByAppointmentAsync(Guid appointmentId);  // latest claim, null if none
    Task<List<InsuranceClaimDto>> GetMyClaimsAsDoctorAsync();
    Task<HttpResponseMessage> ApproveClaimAsync(Guid claimId);
    Task<HttpResponseMessage> DeclineClaimAsync(Guid claimId, string? reason);
    Task<HttpResponseMessage> SettleClaimAsync(Guid claimId, string? insurerReference);
    Task<HttpResponseMessage> RejectClaimAsync(Guid claimId, string? reason);
}
