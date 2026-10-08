using System.Net.Http.Json;
using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public class IdentityApiClient(HttpClient http) : IIdentityApiClient
{
    public Task<HttpResponseMessage> LoginAsync(LoginRequest req) =>
        http.PostAsJsonAsync("api/auth/login", req);

    public Task<HttpResponseMessage> RegisterPatientAsync(RegisterPatientRequest req) =>
        http.PostAsJsonAsync("api/auth/register", req);

    public Task<HttpResponseMessage> RegisterDoctorAsync(RegisterDoctorRequest req) =>
        http.PostAsJsonAsync("api/auth/register-doctor", req);

    public async Task<UserDto?> GetMeAsync()
    {
        var resp = await http.GetAsync("api/auth/me");
        return resp.IsSuccessStatusCode ? await resp.Content.ReadFromJsonAsync<UserDto>() : null;
    }

    public Task<HttpResponseMessage> UpdateMeAsync(UpdateMeRequest req) =>
        http.PutAsJsonAsync("api/auth/me", req, JsonDefaults.Options);

    // Best effort with a short timeout: logging must never hold up signing out
    public async Task ReportSessionEndedAsync(string reason)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            await http.PostAsJsonAsync("api/auth/session-ended", new SessionEndedRequest(reason), JsonDefaults.Options, cts.Token);
        }
        catch (Exception) { }
    }

    public Task<HttpResponseMessage> ChangePasswordAsync(ChangePasswordRequest req) =>
        http.PostAsJsonAsync("api/auth/change-password", req, JsonDefaults.Options);

    public async Task<List<PatientContactDto>> GetMyPatientsAsync() =>
        await http.GetFromJsonAsync<List<PatientContactDto>>("api/patients/mine", JsonDefaults.Options) ?? [];

    public async Task<UserCountsDto?> GetStatsAsync()
    {
        var resp = await http.GetAsync("api/auth/stats");
        return resp.IsSuccessStatusCode ? await resp.Content.ReadFromJsonAsync<UserCountsDto>() : null;
    }

    public async Task<UserDto?> GetUserAsAdminAsync(Guid userId)
    {
        var resp = await http.GetAsync($"api/auth/admin/users/{userId}");
        return resp.IsSuccessStatusCode ? await resp.Content.ReadFromJsonAsync<UserDto>() : null;
    }

    public Task<HttpResponseMessage> UpdateUserAsAdminAsync(Guid userId, UpdateMeRequest req) =>
        http.PutAsJsonAsync($"api/auth/admin/users/{userId}", req, JsonDefaults.Options);

    public async Task<List<UserDto>> GetAdminUsersAsync(string? role)
    {
        var url = string.IsNullOrWhiteSpace(role) ? "api/auth/admin/users" : $"api/auth/admin/users?role={role}";
        return await http.GetFromJsonAsync<List<UserDto>>(url) ?? [];
    }

    public async Task<FamilyOverviewDto?> GetMyFamilyAsync()
    {
        var resp = await http.GetAsync("api/family/mine");
        return resp.IsSuccessStatusCode ? await resp.Content.ReadFromJsonAsync<FamilyOverviewDto>() : null;
    }

    public Task<HttpResponseMessage> InviteFamilyAsync(CreateFamilyInviteRequest req) =>
        http.PostAsJsonAsync("api/family/invites", req);

    public Task<HttpResponseMessage> AcceptFamilyInviteAsync(Guid id) =>
        http.PostAsync($"api/family/invites/{id}/accept", null);

    public Task<HttpResponseMessage> DeclineFamilyInviteAsync(Guid id) =>
        http.PostAsync($"api/family/invites/{id}/decline", null);

    public Task<HttpResponseMessage> RevokeFamilyLinkAsync(Guid id) =>
        http.PostAsync($"api/family/invites/{id}/revoke", null);

    public Task<HttpResponseMessage> AddDependentAsync(AddDependentRequest req) =>
        http.PostAsJsonAsync("api/family/dependents", req);

    public Task<HttpResponseMessage> RemoveDependentAsync(Guid id) =>
        http.DeleteAsync($"api/family/dependents/{id}");
}
