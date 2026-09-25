using System.Text.Json;
using System.Text.Json.Serialization;

namespace LandaDoc.Frontend.Shared.Services;

// Every backend service serializes enums as strings (JsonStringEnumConverter registered
// server-side), but System.Text.Json's client-side GetFromJsonAsync/ReadFromJsonAsync
// default options don't accept string-encoded enums — they throw. Use these options for
// every deserialize call that touches a DTO with an enum field (Status, Type, Day, ...).
public static class JsonDefaults
{
    public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };
}
