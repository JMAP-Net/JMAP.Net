using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Capabilities.Core.Methods;
using JSCalendar.Net;

namespace JMAP.Net.Capabilities.Calendars.Methods.Calendar;

/// <summary>
/// Request for Calendar/set method.
/// As per JMAP Calendars RFC, Section 4.3.
/// </summary>
public class CalendarSetRequest : SetRequest<JMAP.Net.Capabilities.Calendars.Types.Calendar, PatchObject>
{
    /// <summary>
    /// If false, any attempt to destroy a Calendar that still has CalendarEvents will be rejected.
    /// If true, any CalendarEvents in the Calendar will be removed from it, and if in no other
    /// Calendars they will be destroyed.
    /// Default: false.
    /// </summary>
    [JsonPropertyName("onDestroyRemoveEvents")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool OnDestroyRemoveEvents { get; init; } = false;

    /// <summary>
    /// If an id is given, and all creates, updates and destroys succeed, the server will
    /// try to set this calendar as the default.
    /// </summary>
    [JsonPropertyName("onSuccessSetIsDefault")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapId? OnSuccessSetIsDefault { get; init; }
}
