using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public interface ISearchApiClient
{
    Task<List<DoctorSearchResultDto>> SearchDoctorsAsync(string? specialty, string? city, string? q);
    Task<DoctorSearchResultDto?> GetDoctorAsync(Guid doctorId);
}
