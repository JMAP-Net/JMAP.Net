using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;

/// <summary>
/// Request for CalendarEvent/get method.
/// As per JMAP Calendars RFC, Section 5.7.
/// </summary>
public class CalendarEventGetRequest : GetRequest<JMAP.Net.Capabilities.Calendars.Types.CalendarEvent>
{
    /// <summary>
    /// If given, only recurrence overrides with a recurrence id before this date (in UTC)
    /// will be returned.
    /// </summary>
    [JsonPropertyName("recurrenceOverridesBefore")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapUtcDate? RecurrenceOverridesBefore { get; init; }

    /// <summary>
    /// If given, only recurrence overrides with a recurrence id on or after this date (in UTC)
    /// will be returned.
    /// </summary>
    [JsonPropertyName("recurrenceOverridesAfter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapUtcDate? RecurrenceOverridesAfter { get; init; }

    /// <summary>
    /// If true, only participants with the "owner" role or corresponding to the user's
    /// participant identities will be returned.
    /// Default: false.
    /// </summary>
    [JsonPropertyName("reduceParticipants")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool ReduceParticipants { get; init; } = false;

    /// <summary>
    /// The time zone to use when calculating utcStart/utcEnd for floating events.
    /// Default: "Etc/UTC".
    /// </summary>
    [JsonPropertyName("timeZone")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TimeZone { get; init; }
}
