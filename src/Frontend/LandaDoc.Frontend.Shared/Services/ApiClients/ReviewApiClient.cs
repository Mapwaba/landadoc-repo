using System.Net.Http.Json;
using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public class ReviewApiClient(HttpClient http) : IReviewApiClient
{
    public Task<HttpResponseMessage> SubmitAsync(CreateReviewRequest req) =>
        http.PostAsJsonAsync("api/reviews", req);

    public async Task<List<ReviewDto>> GetForDoctorAsync(Guid doctorId) =>
        await http.GetFromJsonAsync<List<ReviewDto>>($"api/reviews/doctor/{doctorId}") ?? [];
}
