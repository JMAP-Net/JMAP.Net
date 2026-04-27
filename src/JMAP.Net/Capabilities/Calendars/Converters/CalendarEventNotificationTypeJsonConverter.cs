using System.Text.Json;
using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Calendars.Types;

namespace JMAP.Net.Capabilities.Calendars.Converters;

/// <summary>
/// JSON converter for <see cref="CalendarEventNotificationType" />.
/// </summary>
public sealed class CalendarEventNotificationTypeJsonConverter : JsonConverter<CalendarEventNotificationType>
{
    /// <inheritdoc />
    public override CalendarEventNotificationType Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "created" => CalendarEventNotificationType.Created,
            "updated" => CalendarEventNotificationType.Updated,
            "destroyed" => CalendarEventNotificationType.Destroyed,
            var value => throw new JsonException($"Unknown calendar event notification type value '{value}'.")
        };
    }

    /// <inheritdoc />
    public override void Write(
        Utf8JsonWriter writer,
        CalendarEventNotificationType value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value switch
        {
            CalendarEventNotificationType.Created => "created",
            CalendarEventNotificationType.Updated => "updated",
            CalendarEventNotificationType.Destroyed => "destroyed",
            _ => throw new JsonException($"Unknown calendar event notification type value '{value}'.")
        });
    }
}
