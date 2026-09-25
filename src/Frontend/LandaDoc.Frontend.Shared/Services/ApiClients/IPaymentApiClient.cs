using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public interface IPaymentApiClient
{
    Task<PaymentDto?> GetByIdAsync(Guid id);
    Task<PaymentDto?> GetByAppointmentAsync(Guid appointmentId);
    Task<HttpResponseMessage> InitiateAsync(Guid paymentId, InitiatePaymentRequest request);
}
