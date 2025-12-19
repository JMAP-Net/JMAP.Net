using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Methods.Query;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;

/// <summary>
/// Request for CalendarEvent/query method.
/// As per JMAP Calendars RFC, Section 5.11.
/// </summary>
public class CalendarEventQueryRequest : QueryRequest<CalendarEventFilterCondition>
{
    /// <summary>
    /// If true, the server will expand recurring events.
    /// If true, the filter MUST be just a FilterCondition (not FilterOperator)
    /// and MUST include both "before" and "after" properties.
    /// Default: false.
    /// </summary>
    [JsonPropertyName("expandRecurrences")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool ExpandRecurrences { get; init; } = false;

    /// <summary>
    /// The time zone for before/after filter conditions.
    /// Default: "Etc/UTC".
    /// </summary>
    [JsonPropertyName("timeZone")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TimeZone { get; init; }
}
