using System.Text.Json.Serialization;

namespace JMAP.Net.Capabilities.Calendars.Types;

/// <summary>
/// JMAP extension of JSCalendar Participant with scheduling-specific properties.
/// Use this instead of JSCalendar.Net.Participant when working with JMAP Calendars
/// and scheduling functionality is required.
/// As per RFC 8984, Section 5.1.4.
/// </summary>
public class JmapParticipant : JSCalendar.Net.Participant
{
    /// <summary>
    /// A URI as defined by RFC 3986 or any other IANA-registered form for a URI.
    /// It is the same as the CAL-ADDRESS value of an ATTENDEE or ORGANIZER in 
    /// iCalendar (RFC 5545) — it globally identifies a particular participant, 
    /// even across different events.
    /// Examples: "mailto:alice@example.com", "https://example.com/principals/alice"
    /// </summary>
    [JsonPropertyName("scheduleId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ScheduleId { get; init; }
}
