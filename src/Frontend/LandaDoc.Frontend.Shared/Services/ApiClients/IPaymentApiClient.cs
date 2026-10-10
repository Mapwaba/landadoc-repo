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
    // 204 when deleted; 200 with a DeleteInsurerResult (SwitchedOff) when claims were made with it
    Task<HttpResponseMessage> DeleteInsurerAsync(Guid id);
    // The active insurers a doctor takes (what a patient may choose for that doctor)
    Task<List<InsurerDto>> GetInsurersForDoctorAsync(Guid doctorId);
    // The calling doctor's own choice of insurers
    Task<AcceptedInsurersDto> GetMyAcceptedInsurersAsync();
    Task<HttpResponseMessage> SaveMyAcceptedInsurersAsync(AcceptedInsurersDto choice);

    // Insurance claims. The patient files one through InitiateAsync (Provider = Insurance);
    // the doctor then approves/declines it and later records whether the insurer paid.
    Task<InsuranceClaimDto?> GetClaimByAppointmentAsync(Guid appointmentId);  // latest claim, null if none
    Task<List<InsuranceClaimDto>> GetMyClaimsAsDoctorAsync();
    // authorizationReference: what the insurer gave the doctor when confirming the cover (required)
    Task<HttpResponseMessage> ApproveClaimAsync(Guid claimId, string authorizationReference);
    Task<HttpResponseMessage> DeclineClaimAsync(Guid claimId, string? reason);
    Task<HttpResponseMessage> SettleClaimAsync(Guid claimId, string? insurerReference);
    Task<HttpResponseMessage> RejectClaimAsync(Guid claimId, string? reason);

    // Doctor payouts. The doctor sees their balance and payouts and gives the account to be paid
    // on; admins check that account and record each payout they send.
    Task<DoctorPayoutsDto?> GetMyPayoutsAsync();
    Task<HttpResponseMessage> SaveMyPayoutAccountAsync(SavePayoutAccountRequest request);
    Task<List<DoctorBalanceDto>> GetPayoutBalancesAsync();
    // LandaDoc's wallets at Moko Afrika (FreshPay); null when they couldn't be read
    Task<List<MobileMoneyWalletDto>?> GetMobileMoneyWalletsAsync();
    Task<DoctorPayoutsDto?> GetDoctorPayoutsAsync(Guid doctorId);
    Task<HttpResponseMessage> VerifyPayoutAccountAsync(Guid doctorId);
    Task<HttpResponseMessage> RecordPayoutAsync(Guid doctorId, RecordPayoutRequest request);
}
