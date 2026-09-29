namespace LandaDoc.Frontend.Shared.Services;

// Specialties offered in the doctor profile, admin and patient search dropdowns.
// The English name is what gets stored and searched on; show it through Lang[...]
// so French users see the translated label (Translations.cs).
public static class Specialties
{
    public static readonly IReadOnlyList<string> All =
    [
        "Cardiologist",
        "Neurologist",
        "Generalist",
        "Ophtamologist",
        "Pediatrician",
    ];
}
