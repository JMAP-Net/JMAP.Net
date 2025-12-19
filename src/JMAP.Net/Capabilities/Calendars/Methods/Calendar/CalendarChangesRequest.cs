using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.Calendar;

/// <summary>
/// Request for Calendar/changes method.
/// As per JMAP Calendars RFC, Section 4.2.
/// </summary>
public class CalendarChangesRequest : ChangesRequest
{
    // Inherits all properties from ChangesRequest:
    // - AccountId
    // - SinceState
    // - MaxChanges (optional)
}
