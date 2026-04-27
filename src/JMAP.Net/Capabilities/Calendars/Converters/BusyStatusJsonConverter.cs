using System.Text.Json;
using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Calendars.Types;

namespace JMAP.Net.Capabilities.Calendars.Converters;

/// <summary>
/// JSON converter for <see cref="BusyStatus" />.
/// </summary>
public sealed class BusyStatusJsonConverter : JsonConverter<BusyStatus>
{
    /// <inheritdoc />
    public override BusyStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "confirmed" => BusyStatus.Confirmed,
            "tentative" => BusyStatus.Tentative,
            "unavailable" => BusyStatus.Unavailable,
            var value => throw new JsonException($"Unknown busy status value '{value}'.")
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, BusyStatus value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value switch
        {
            BusyStatus.Confirmed => "confirmed",
            BusyStatus.Tentative => "tentative",
            BusyStatus.Unavailable => "unavailable",
            _ => throw new JsonException($"Unknown busy status value '{value}'.")
        });
    }
}
