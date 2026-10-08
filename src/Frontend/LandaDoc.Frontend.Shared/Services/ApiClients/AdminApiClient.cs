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

    public async Task<MyProfileLookup> LookUpMyProfileAsync()
    {
        try
        {
            var resp = await http.GetAsync("api/doctors/me");
            if (resp.StatusCode == System.Net.HttpStatusCode.NotFound) return new MyProfileLookup(true, null);
            if (!resp.IsSuccessStatusCode) return new MyProfileLookup(false, null);
            return new MyProfileLookup(true, await resp.Content.ReadFromJsonAsync<DoctorProfileDto>(JsonDefaults.Options));
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            return new MyProfileLookup(false, null);
        }
    }

    public Task<HttpResponseMessage> CreateMyProfileAsync(CreateOwnDoctorProfileRequest req) =>
        http.PostAsJsonAsync("api/doctors/me", req);

    public Task<HttpResponseMessage> UpdateMyProfileAsync(UpdateOwnDoctorProfileRequest req) =>
        http.PutAsJsonAsync("api/doctors/me", req);

    public async Task<List<ClinicDto>> GetClinicsAsync() =>
        await http.GetFromJsonAsync<List<ClinicDto>>("api/clinics", JsonDefaults.Options) ?? [];

    public async Task<List<WorkplaceDto>> GetMyWorkplacesAsync()
    {
        var resp = await http.GetAsync("api/doctors/me/workplaces");
        return resp.IsSuccessStatusCode
            ? await resp.Content.ReadFromJsonAsync<List<WorkplaceDto>>(JsonDefaults.Options) ?? []
            : []; // 404 = no doctor profile yet
    }

    public Task<HttpResponseMessage> UpdateMyWorkplaceAsync(Guid clinicId, UpdateWorkplaceRequest req) =>
        http.PutAsJsonAsync($"api/doctors/me/workplaces/{clinicId}", req, JsonDefaults.Options);

    public async Task<List<DoctorServiceDto>> GetMyServicesAsync()
    {
        var resp = await http.GetAsync("api/doctors/me/services");
        return resp.IsSuccessStatusCode
            ? await resp.Content.ReadFromJsonAsync<List<DoctorServiceDto>>(JsonDefaults.Options) ?? []
            : [];
    }

    public Task<HttpResponseMessage> CreateMyServiceAsync(SaveDoctorServiceRequest req) =>
        http.PostAsJsonAsync("api/doctors/me/services", req, JsonDefaults.Options);

    public Task<HttpResponseMessage> UpdateMyServiceAsync(Guid id, SaveDoctorServiceRequest req) =>
        http.PutAsJsonAsync($"api/doctors/me/services/{id}", req, JsonDefaults.Options);

    public Task<HttpResponseMessage> DeleteMyServiceAsync(Guid id) =>
        http.DeleteAsync($"api/doctors/me/services/{id}");

    public async Task<List<ClinicOptionDto>> GetClinicOptionsAsync() =>
        await http.GetFromJsonAsync<List<ClinicOptionDto>>("api/clinics/options", JsonDefaults.Options) ?? [];

    public async Task<int> GetApprovedDoctorCountAsync() =>
        await http.GetFromJsonAsync<int>("api/doctors/count");

    public async Task<List<DoctorProfileDto>> GetAllDoctorsAsync(DoctorApprovalStatus? status)
    {
        var url = status is null ? "api/admin/doctors" : $"api/admin/doctors?status={status}";
        return await http.GetFromJsonAsync<List<DoctorProfileDto>>(url, JsonDefaults.Options) ?? [];
    }

    public Task<HttpResponseMessage> CreateDoctorAsync(CreateDoctorProfileRequest req) =>
        http.PostAsJsonAsync("api/admin/doctors", req);

    public async Task<DoctorProfileDto?> GetDoctorAsync(Guid id)
    {
        var resp = await http.GetAsync($"api/admin/doctors/{id}");
        return resp.IsSuccessStatusCode
            ? await resp.Content.ReadFromJsonAsync<DoctorProfileDto>(JsonDefaults.Options)
            : null;
    }

    public Task<HttpResponseMessage> UpdateDoctorAsync(Guid id, UpdateOwnDoctorProfileRequest req) =>
        http.PutAsJsonAsync($"api/admin/doctors/{id}", req, JsonDefaults.Options);

    public Task<HttpResponseMessage> ApproveDoctorAsync(Guid id) =>
        http.PatchAsync($"api/admin/doctors/{id}/approve", null);

    public Task<HttpResponseMessage> SuspendDoctorAsync(Guid id, SuspendDoctorRequest req) =>
        http.PatchAsJsonAsync($"api/admin/doctors/{id}/suspend", req);

    public Task<HttpResponseMessage> CreateClinicAsync(CreateClinicRequest req) =>
        http.PostAsJsonAsync("api/clinics", req);

    public Task<HttpResponseMessage> UpdateClinicAsync(Guid id, UpdateClinicRequest req) =>
        http.PatchAsJsonAsync($"api/clinics/{id}", req);
}
