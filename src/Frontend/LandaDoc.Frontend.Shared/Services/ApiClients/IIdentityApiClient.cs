using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public interface IIdentityApiClient
{
    Task<HttpResponseMessage> LoginAsync(LoginRequest req);
    Task<HttpResponseMessage> RegisterPatientAsync(RegisterPatientRequest req);
    Task<HttpResponseMessage> RegisterDoctorAsync(RegisterDoctorRequest req);
    Task<UserDto?> GetMeAsync();
    Task<UserCountsDto?> GetStatsAsync();
    Task<List<UserDto>> GetAdminUsersAsync(string? role);

    Task<FamilyOverviewDto?> GetMyFamilyAsync();
    Task<HttpResponseMessage> InviteFamilyAsync(CreateFamilyInviteRequest req);
    Task<HttpResponseMessage> AcceptFamilyInviteAsync(Guid id);
    Task<HttpResponseMessage> DeclineFamilyInviteAsync(Guid id);
    Task<HttpResponseMessage> RevokeFamilyLinkAsync(Guid id);
    Task<HttpResponseMessage> AddDependentAsync(AddDependentRequest req);
    Task<HttpResponseMessage> RemoveDependentAsync(Guid id);
}
