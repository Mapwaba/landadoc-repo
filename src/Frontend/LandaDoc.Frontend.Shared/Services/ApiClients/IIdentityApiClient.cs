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
    // Tells Identity why the browser is ending the session, for the logs ("inactive", ...)
    Task ReportSessionEndedAsync(string reason);
    Task<UserCountsDto?> GetStatsAsync();
    // Doctor: contact details of the patients they've had an appointment with (throws if unavailable)
    Task<List<PatientContactDto>> GetMyPatientsAsync();
    // Doctor: names of everyone on their appointments, dependants included
    Task<List<PatientNameDto>> GetMyPatientNamesAsync();
    Task<List<UserDto>> GetAdminUsersAsync(string? role);
    // Admin: read or correct one account's name and phone (e.g. a doctor's, from the Admin app)
    Task<UserDto?> GetUserAsAdminAsync(Guid userId);
    Task<HttpResponseMessage> UpdateUserAsAdminAsync(Guid userId, UpdateMeRequest req);

    Task<FamilyOverviewDto?> GetMyFamilyAsync();
    Task<HttpResponseMessage> InviteFamilyAsync(CreateFamilyInviteRequest req);
    Task<HttpResponseMessage> AcceptFamilyInviteAsync(Guid id);
    Task<HttpResponseMessage> DeclineFamilyInviteAsync(Guid id);
    Task<HttpResponseMessage> RevokeFamilyLinkAsync(Guid id);
    Task<HttpResponseMessage> AddDependentAsync(AddDependentRequest req);
    Task<HttpResponseMessage> RemoveDependentAsync(Guid id);
}
