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

    public async Task<UserCountsDto?> GetStatsAsync()
    {
        var resp = await http.GetAsync("api/auth/stats");
        return resp.IsSuccessStatusCode ? await resp.Content.ReadFromJsonAsync<UserCountsDto>() : null;
    }

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
