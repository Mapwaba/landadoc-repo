using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace LandaDoc.Frontend.Shared.Services;

// TokenService (Identity) mints claims with the full ClaimTypes URIs (e.g.
// ".../claims/nameidentifier"), so the JWT payload's JSON keys already ARE
// those literal claim types — no short/long-name mapping needed here.
public static class JwtParser
{
    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var json = Encoding.UTF8.GetString(Base64UrlDecode(payload));
        var kvPairs = JsonSerializer.Deserialize<Dictionary<string, object>>(json)!;

        foreach (var (key, value) in kvPairs)
        {
            if (value is JsonElement { ValueKind: JsonValueKind.Array } arr)
            {
                foreach (var item in arr.EnumerateArray())
                    yield return new Claim(key, item.ToString());
            }
            else
            {
                yield return new Claim(key, value.ToString() ?? "");
            }
        }
    }

    public static DateTimeOffset? GetExpiry(string jwt)
    {
        var exp = ParseClaimsFromJwt(jwt).FirstOrDefault(c => c.Type == "exp");
        return exp is null ? null : DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp.Value));
    }

    private static byte[] Base64UrlDecode(string input)
    {
        var padded = input.Replace('-', '+').Replace('_', '/');
        padded += new string('=', (4 - padded.Length % 4) % 4);
        return Convert.FromBase64String(padded);
    }
}
