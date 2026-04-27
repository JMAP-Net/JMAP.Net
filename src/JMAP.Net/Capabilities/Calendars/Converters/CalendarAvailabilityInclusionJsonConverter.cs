using System.Text.Json;
using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Calendars.Types;

namespace JMAP.Net.Capabilities.Calendars.Converters;

/// <summary>
/// JSON converter for <see cref="CalendarAvailabilityInclusion" />.
/// </summary>
public sealed class CalendarAvailabilityInclusionJsonConverter : JsonConverter<CalendarAvailabilityInclusion>
{
    /// <inheritdoc />
    public override CalendarAvailabilityInclusion Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "all" => CalendarAvailabilityInclusion.All,
            "attending" => CalendarAvailabilityInclusion.Attending,
            "none" => CalendarAvailabilityInclusion.None,
            var value => throw new JsonException($"Unknown calendar availability inclusion value '{value}'.")
        };
    }

    /// <inheritdoc />
    public override void Write(
        Utf8JsonWriter writer,
        CalendarAvailabilityInclusion value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value switch
        {
            CalendarAvailabilityInclusion.All => "all",
            CalendarAvailabilityInclusion.Attending => "attending",
            CalendarAvailabilityInclusion.None => "none",
            _ => throw new JsonException($"Unknown calendar availability inclusion value '{value}'.")
        });
    }
}
