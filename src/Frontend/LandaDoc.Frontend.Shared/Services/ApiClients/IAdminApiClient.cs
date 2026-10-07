using LandaDoc.Shared.DTOs;
using LandaDoc.Shared.Models;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public interface IAdminApiClient
{
    Task<DoctorProfileDto?> GetMyProfileAsync();
    Task<HttpResponseMessage> CreateMyProfileAsync(CreateOwnDoctorProfileRequest req);
    Task<HttpResponseMessage> UpdateMyProfileAsync(UpdateOwnDoctorProfileRequest req);
    Task<List<ClinicDto>> GetClinicsAsync();

    // Public: id/name/city of every clinic, for the doctor registration form
    Task<List<ClinicOptionDto>> GetClinicOptionsAsync();

    // Public: number of approved doctors, read where approvals are saved so it's never stale
    Task<int> GetApprovedDoctorCountAsync();

    // Admin-only doctor management
    Task<List<DoctorProfileDto>> GetAllDoctorsAsync(DoctorApprovalStatus? status);
    Task<HttpResponseMessage> CreateDoctorAsync(CreateDoctorProfileRequest req);
    Task<HttpResponseMessage> ApproveDoctorAsync(Guid id);
    Task<HttpResponseMessage> SuspendDoctorAsync(Guid id, SuspendDoctorRequest req);

    // Admin-only clinic management
    Task<HttpResponseMessage> CreateClinicAsync(CreateClinicRequest req);
    Task<HttpResponseMessage> UpdateClinicAsync(Guid id, UpdateClinicRequest req);
}
