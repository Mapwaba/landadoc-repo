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
}
