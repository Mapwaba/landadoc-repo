using System.Net.Http.Json;
using System.Web;
using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public class SearchApiClient(HttpClient http) : ISearchApiClient
{
    public async Task<List<DoctorSearchResultDto>> SearchDoctorsAsync(string? specialty, string? city, string? q)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (!string.IsNullOrWhiteSpace(specialty)) query["specialty"] = specialty;
        if (!string.IsNullOrWhiteSpace(city)) query["city"] = city;
        if (!string.IsNullOrWhiteSpace(q)) query["q"] = q;

        return await http.GetFromJsonAsync<List<DoctorSearchResultDto>>(
            $"api/search/doctors?{query}", JsonDefaults.Options) ?? [];
    }

    public async Task<DoctorSearchResultDto?> GetDoctorAsync(Guid doctorId)
    {
        var resp = await http.GetAsync($"api/search/doctors/{doctorId}");
        return resp.IsSuccessStatusCode
            ? await resp.Content.ReadFromJsonAsync<DoctorSearchResultDto>(JsonDefaults.Options)
            : null;
    }
}
