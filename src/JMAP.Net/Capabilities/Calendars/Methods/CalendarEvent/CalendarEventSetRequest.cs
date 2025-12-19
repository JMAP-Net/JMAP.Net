using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Methods;
using JSCalendar.Net;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;

/// <summary>
/// Request for CalendarEvent/set method.
/// As per JMAP Calendars RFC, Section 5.9.
/// </summary>
public class CalendarEventSetRequest : SetRequest<JMAP.Net.Capabilities.Calendars.Types.CalendarEvent, PatchObject>
{
    /// <summary>
    /// If true, any changes to scheduled events will be sent to all participants
    /// (if the server is the origin) or back to the origin (otherwise).
    /// If false, changes only affect this account and no scheduling messages will be sent.
    /// Default: false.
    /// </summary>
    [JsonPropertyName("sendSchedulingMessages")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool SendSchedulingMessages { get; init; } = false;
}
