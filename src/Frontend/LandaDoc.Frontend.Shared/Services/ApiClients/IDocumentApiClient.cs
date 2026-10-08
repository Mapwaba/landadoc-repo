using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public interface IDocumentApiClient
{
    Task<HttpResponseMessage> UploadAsync(
        Guid patientId, Guid? doctorId, Guid? appointmentId, string category,
        Stream fileStream, string fileName, string contentType);
    Task<List<DocumentDto>> GetForPatientAsync(Guid patientId);
    Task<List<DocumentDto>> GetForAppointmentAsync(Guid appointmentId);
    Task<string?> GetDownloadUrlAsync(Guid documentId);

    // A doctor's verification documents (VerificationDocumentKinds): the doctor manages their
    // own, admins read them when reviewing the doctor
    Task<HttpResponseMessage> UploadVerificationDocumentAsync(string kind, Stream fileStream, string fileName, string contentType);
    Task<List<VerificationDocumentDto>> GetMyVerificationDocumentsAsync();
    Task<List<VerificationDocumentDto>> GetVerificationDocumentsForDoctorAsync(Guid doctorUserId);
    Task<string?> GetVerificationDocumentUrlAsync(Guid documentId);
    Task<HttpResponseMessage> DeleteVerificationDocumentAsync(Guid documentId);
}
