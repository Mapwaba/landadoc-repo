using System.Net.Http.Json;
using LandaDoc.Shared.DTOs;
using LandaDoc.Shared.Models;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public class AdminApiClient(HttpClient http) : IAdminApiClient
{
    public async Task<DoctorProfileDto?> GetMyProfileAsync()
    {
        var resp = await http.GetAsync("api/doctors/me");
        return resp.IsSuccessStatusCode
            ? await resp.Content.ReadFromJsonAsync<DoctorProfileDto>(JsonDefaults.Options)
            : null;
    }

    public Task<HttpResponseMessage> CreateMyProfileAsync(CreateOwnDoctorProfileRequest req) =>
        http.PostAsJsonAsync("api/doctors/me", req);

    public Task<HttpResponseMessage> UpdateMyProfileAsync(UpdateOwnDoctorProfileRequest req) =>
        http.PutAsJsonAsync("api/doctors/me", req);

    public async Task<List<ClinicDto>> GetClinicsAsync() =>
        await http.GetFromJsonAsync<List<ClinicDto>>("api/clinics", JsonDefaults.Options) ?? [];

    public async Task<List<DoctorProfileDto>> GetAllDoctorsAsync(DoctorApprovalStatus? status)
    {
        var url = status is null ? "api/admin/doctors" : $"api/admin/doctors?status={status}";
        return await http.GetFromJsonAsync<List<DoctorProfileDto>>(url, JsonDefaults.Options) ?? [];
    }

    public Task<HttpResponseMessage> CreateDoctorAsync(CreateDoctorProfileRequest req) =>
        http.PostAsJsonAsync("api/admin/doctors", req);

    public Task<HttpResponseMessage> ApproveDoctorAsync(Guid id) =>
        http.PatchAsync($"api/admin/doctors/{id}/approve", null);

    public Task<HttpResponseMessage> SuspendDoctorAsync(Guid id, SuspendDoctorRequest req) =>
        http.PatchAsJsonAsync($"api/admin/doctors/{id}/suspend", req);

    public Task<HttpResponseMessage> CreateClinicAsync(CreateClinicRequest req) =>
        http.PostAsJsonAsync("api/clinics", req);

    public Task<HttpResponseMessage> UpdateClinicAsync(Guid id, UpdateClinicRequest req) =>
        http.PatchAsJsonAsync($"api/clinics/{id}", req);
}
