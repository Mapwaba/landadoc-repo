using LandaDoc.Shared.DTOs;
using LandaDoc.Shared.Models;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public record MyProfileLookup(bool Reachable, DoctorProfileDto? Profile);

public interface IAdminApiClient
{
    Task<DoctorProfileDto?> GetMyProfileAsync();
    // Like GetMyProfileAsync, but tells "no profile yet" (Reachable, Profile null) apart from
    // "couldn't ask" (not Reachable) — used to gate the Doctor app while approval is pending
    Task<MyProfileLookup> LookUpMyProfileAsync();
    Task<HttpResponseMessage> CreateMyProfileAsync(CreateOwnDoctorProfileRequest req);
    Task<HttpResponseMessage> UpdateMyProfileAsync(UpdateOwnDoctorProfileRequest req);
    Task<List<ClinicDto>> GetClinicsAsync();

    // Doctor: the establishments they work at, and the services they offer
    Task<List<WorkplaceDto>> GetMyWorkplacesAsync();
    Task<HttpResponseMessage> UpdateMyWorkplaceAsync(Guid clinicId, UpdateWorkplaceRequest req);
    Task<List<DoctorServiceDto>> GetMyServicesAsync();
    Task<HttpResponseMessage> CreateMyServiceAsync(SaveDoctorServiceRequest req);
    Task<HttpResponseMessage> UpdateMyServiceAsync(Guid id, SaveDoctorServiceRequest req);
    Task<HttpResponseMessage> DeleteMyServiceAsync(Guid id);

    // Public: id/name/city of every clinic, for the doctor registration form
    Task<List<ClinicOptionDto>> GetClinicOptionsAsync();

    // Public: number of approved doctors, read where approvals are saved so it's never stale
    Task<int> GetApprovedDoctorCountAsync();

    // Admin-only doctor management
    Task<List<DoctorProfileDto>> GetAllDoctorsAsync(DoctorApprovalStatus? status);
    Task<DoctorProfileDto?> GetDoctorAsync(Guid id);   // null if there's no such profile
    // Same fields as the doctor's own edit; name and phone go to Identity separately
    Task<HttpResponseMessage> UpdateDoctorAsync(Guid id, UpdateOwnDoctorProfileRequest req);
    Task<HttpResponseMessage> CreateDoctorAsync(CreateDoctorProfileRequest req);
    Task<HttpResponseMessage> ApproveDoctorAsync(Guid id);
    Task<HttpResponseMessage> SuspendDoctorAsync(Guid id, SuspendDoctorRequest req);

    // Admin-only clinic management
    Task<HttpResponseMessage> CreateClinicAsync(CreateClinicRequest req);
    Task<HttpResponseMessage> UpdateClinicAsync(Guid id, UpdateClinicRequest req);
}
