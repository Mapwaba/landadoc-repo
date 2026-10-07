using System.Net;
using System.Text.Json;

namespace LandaDoc.Frontend.Shared.Services;

// Identity and Admin answer 409 with { "code": "email" | "phone" | "name" } when a value belongs
// to another account; this turns that into the message to show. Null when it isn't such a 409.
public static class DuplicateAccountMessages
{
    public static async Task<string?> ForAsync(HttpResponseMessage resp, ILanguageService lang, bool isDoctor)
    {
        if (resp.StatusCode != HttpStatusCode.Conflict) return null;

        string? code = null;
        try
        {
            using var json = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
            if (json.RootElement.TryGetProperty("code", out var c)) code = c.GetString();
        }
        catch (JsonException) { }

        return code switch
        {
            "phone" => lang["This phone number already exists."],
            "name" => isDoctor
                ? lang["A doctor with this first and last name already exists."]
                : lang["A patient with this first and last name already exists."],
            "email" => lang["This email address already exists."],
            _ => null, // some other conflict (e.g. "you already have a profile")
        };
    }
}
