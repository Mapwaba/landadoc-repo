using LandaDoc.Frontend.Shared.Services.ApiClients;
using LandaDoc.Shared.DTOs;

namespace LandaDoc.Frontend.Shared.Services;

// The Appointment service only knows patient ids, so a doctor's appointments come back without
// names. This loads the appointments and the patient names (from Identity) side by side and fills
// PatientName in, so the agenda, dashboard and payments show "Jean Mubenga" instead of "Patient".
public static class DoctorAppointments
{
    public static async Task<List<AppointmentDto>> GetMineWithPatientNamesAsync(
        IAppointmentApiClient appointmentsApi, IIdentityApiClient identity)
    {
        var appointmentsTask = appointmentsApi.GetMineAsync();
        var namesTask = identity.GetMyPatientNamesAsync();
        var appointments = await appointmentsTask;

        Dictionary<Guid, string> names;
        try
        {
            names = (await namesTask).ToDictionary(n => n.Id, n => $"{n.FirstName} {n.LastName}".Trim());
        }
        catch (Exception)
        {
            return appointments; // names are a nicety: without Identity the pages fall back to "Patient"
        }

        return appointments
            .Select(a => a.PatientName is null && names.TryGetValue(a.PatientId, out var name) ? a with { PatientName = name } : a)
            .ToList();
    }
}
