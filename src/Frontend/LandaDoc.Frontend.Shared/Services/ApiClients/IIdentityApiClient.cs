using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public interface IIdentityApiClient
{
    Task<HttpResponseMessage> LoginAsync(LoginRequest req);
    Task<HttpResponseMessage> RegisterPatientAsync(RegisterPatientRequest req);
    Task<HttpResponseMessage> RegisterDoctorAsync(RegisterDoctorRequest req);
    Task<UserDto?> GetMeAsync();
    Task<HttpResponseMessage> UpdateMeAsync(UpdateMeRequest req);
    // 200 with a fresh AuthResponse, or 400 when the current password is wrong
    Task<HttpResponseMessage> ChangePasswordAsync(ChangePasswordRequest req);
    Task<UserCountsDto?> GetStatsAsync();
    // Doctor: contact details of the patients they've had an appointment with (throws if unavailable)
    Task<List<PatientContactDto>> GetMyPatientsAsync();
    Task<List<UserDto>> GetAdminUsersAsync(string? role);

    Task<FamilyOverviewDto?> GetMyFamilyAsync();
    Task<HttpResponseMessage> InviteFamilyAsync(CreateFamilyInviteRequest req);
    Task<HttpResponseMessage> AcceptFamilyInviteAsync(Guid id);
    Task<HttpResponseMessage> DeclineFamilyInviteAsync(Guid id);
    Task<HttpResponseMessage> RevokeFamilyLinkAsync(Guid id);
    Task<HttpResponseMessage> AddDependentAsync(AddDependentRequest req);
    Task<HttpResponseMessage> RemoveDependentAsync(Guid id);
}
