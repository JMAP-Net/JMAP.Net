using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Calendars.Types;

/// <summary>
/// Filter condition for CalendarEventNotification/query.
/// As per RFC 8984, Section 6.4.
/// </summary>
public class CalendarEventNotificationFilterCondition
{
    /// <summary>
    /// The creation date must be on or after this date to match the condition.
    /// </summary>
    [JsonPropertyName("after")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapUtcDate? After { get; init; }

    /// <summary>
    /// The creation date must be before this date to match the condition.
    /// </summary>
    [JsonPropertyName("before")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapUtcDate? Before { get; init; }

    /// <summary>
    /// The type property must match to satisfy the condition.
    /// Must be one of: "created", "updated", "destroyed".
    /// </summary>
    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CalendarEventNotificationType? Type { get; init; }

    /// <summary>
    /// A list of event ids. The calendarEventId property of the notification 
    /// must be in this list to match the condition.
    /// </summary>
    [JsonPropertyName("calendarEventIds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<JmapId>? CalendarEventIds { get; init; }
}
