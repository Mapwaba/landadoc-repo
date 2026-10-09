using System.Globalization;
using LandaDoc.Shared.Models;

namespace LandaDoc.Frontend.Shared.Services;

// Appointment times are on the doctor's clock (where they work). When the viewer's clock reads
// differently — a relative booking from abroad — the Patient app also shows the viewer's own time:
// "16:00 · 11:00 your time", under a note saying whose time the slots are in.
public static class SlotTimes
{
    // The viewer's time for a slot ("11:00", or "Sat 23:00" when it falls on another day of
    // theirs); null when both clocks read the same, so nothing extra is shown
    public static string? ForViewer(DateOnly date, string slot, string? doctorZone, string language, TimeZoneInfo? viewerZone = null)
    {
        if (doctorZone is null || !TimeOnly.TryParse(slot, CultureInfo.InvariantCulture, out var time)) return null;
        var viewer = LocalClock.OnClock(date, time, doctorZone, viewerZone ?? TimeZoneInfo.Local);
        if (viewer == date.ToDateTime(time)) return null;
        return DateOnly.FromDateTime(viewer) == date
            ? viewer.ToString("HH:mm", CultureInfo.InvariantCulture)
            : viewer.ToString("ddd HH:mm", CultureInfo.GetCultureInfo(language == "fr" ? "fr-FR" : "en-GB"));
    }

    // "16:00 · 11:00 your time", or just "16:00" for a viewer on the doctor's clock
    public static string Label(DateOnly date, string slot, string? doctorZone, ILanguageService lang) =>
        ForViewer(date, slot, doctorZone, lang.CurrentLanguage) is { } yours
            ? $"{slot} · {string.Format(lang["yourTime"], yours)}"
            : slot;

    // "Times are Kinshasa time, where the doctor works…", when any slot reads differently for the viewer
    public static string? Note(DateOnly date, IEnumerable<string> slots, string? doctorZone, ILanguageService lang) =>
        slots.Any(s => ForViewer(date, s, doctorZone, lang.CurrentLanguage) is not null)
            ? string.Format(lang["slotZoneNote"], LocalClock.CityOf(doctorZone))
            : null;
}
