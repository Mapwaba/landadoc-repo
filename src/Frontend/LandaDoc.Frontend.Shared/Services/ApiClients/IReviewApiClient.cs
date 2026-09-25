using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public interface IReviewApiClient
{
    Task<HttpResponseMessage> SubmitAsync(CreateReviewRequest req);
    Task<List<ReviewDto>> GetForDoctorAsync(Guid doctorId);
}
