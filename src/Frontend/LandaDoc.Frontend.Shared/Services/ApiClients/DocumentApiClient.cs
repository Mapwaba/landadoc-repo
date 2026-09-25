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

    public async Task<string?> GetDownloadUrlAsync(Guid documentId)
    {
        var resp = await http.GetAsync($"api/documents/{documentId}/download-url");
        if (!resp.IsSuccessStatusCode) return null;
        var json = await resp.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        return json?.GetValueOrDefault("url");
    }
}
