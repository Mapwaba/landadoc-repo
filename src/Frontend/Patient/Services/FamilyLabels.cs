using LandaDoc.Frontend.Shared.Services;
using LandaDoc.Frontend.Shared.Services.ApiClients;

namespace LandaDoc.Patient.Services;

// "Ruth Mbuyi (your daughter)" for each person the patient can book for — linked family members
// and dependants — to label appointments booked for a relative. Empty if Identity doesn't answer.
public static class FamilyLabels
{
    public static async Task<Dictionary<Guid, string>> LoadAsync(IIdentityApiClient identity)
    {
        try
        {
            var family = await identity.GetMyFamilyAsync();
            if (family is null) return [];
            return family.Family.Select(f => (f.UserId, Label: $"{f.FirstName} {f.LastName} ({f.RelationLabel})"))
                .Concat(family.Dependents.Select(d => (UserId: d.Id, Label: $"{d.FirstName} {d.LastName} ({d.RelationLabel})")))
                .GroupBy(x => x.UserId)
                .ToDictionary(g => g.Key, g => g.First().Label);
        }
        catch (Exception)
        {
            return [];
        }
    }

    // The label for an appointment's patient: null when it's the signed-in patient themselves
    public static string? For(Guid patientId, Guid me, Dictionary<Guid, string> labels, ILanguageService lang) =>
        patientId == me ? null : labels.GetValueOrDefault(patientId) ?? lang["A family member"];
}
