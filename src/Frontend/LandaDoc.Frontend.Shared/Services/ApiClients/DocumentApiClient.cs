using System.Net.Http.Json;
using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services.ApiClients;

public class DocumentApiClient(HttpClient http) : IDocumentApiClient
{
    public async Task<HttpResponseMessage> UploadAsync(
        Guid patientId, Guid? doctorId, Guid? appointmentId, string category,
        Stream fileStream, string fileName, string contentType)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(patientId.ToString()), "patientId" },
            { new StringContent(category), "category" }
        };
        if (doctorId is not null) content.Add(new StringContent(doctorId.ToString()!), "doctorId");
        if (appointmentId is not null) content.Add(new StringContent(appointmentId.ToString()!), "appointmentId");

        using var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
        content.Add(fileContent, "file", fileName);

        return await http.PostAsync("api/documents", content);
    }

    public async Task<List<DocumentDto>> GetForPatientAsync(Guid patientId) =>
        await http.GetFromJsonAsync<List<DocumentDto>>($"api/documents?patientId={patientId}") ?? [];

    public async Task<List<DocumentDto>> GetForAppointmentAsync(Guid appointmentId) =>
        await http.GetFromJsonAsync<List<DocumentDto>>($"api/documents?appointmentId={appointmentId}") ?? [];

    public Task<string?> GetDownloadUrlAsync(Guid documentId) => UrlAsync($"api/documents/{documentId}/download-url");

    public async Task<HttpResponseMessage> UploadVerificationDocumentAsync(string kind, Stream fileStream, string fileName, string contentType)
    {
        using var content = new MultipartFormDataContent { { new StringContent(kind), "kind" } };
        using var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
        content.Add(fileContent, "file", fileName);
        return await http.PostAsync("api/verification-documents", content);
    }

    public async Task<List<VerificationDocumentDto>> GetMyVerificationDocumentsAsync() =>
        await http.GetFromJsonAsync<List<VerificationDocumentDto>>("api/verification-documents/me") ?? [];

    public async Task<List<VerificationDocumentDto>> GetVerificationDocumentsForDoctorAsync(Guid doctorUserId) =>
        await http.GetFromJsonAsync<List<VerificationDocumentDto>>($"api/verification-documents/doctor/{doctorUserId}") ?? [];

    public Task<string?> GetVerificationDocumentUrlAsync(Guid documentId) =>
        UrlAsync($"api/verification-documents/{documentId}/download-url");

    public Task<HttpResponseMessage> DeleteVerificationDocumentAsync(Guid documentId) =>
        http.DeleteAsync($"api/verification-documents/{documentId}");

    // A short-lived link to the file itself, or null if it can't be had
    private async Task<string?> UrlAsync(string path)
    {
        var resp = await http.GetAsync(path);
        if (!resp.IsSuccessStatusCode) return null;
        var json = await resp.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        return json?.GetValueOrDefault("url");
    }
}
