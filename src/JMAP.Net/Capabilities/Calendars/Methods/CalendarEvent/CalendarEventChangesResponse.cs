using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;

/// <summary>
/// Response for CalendarEvent/changes method.
/// As per JMAP Calendars RFC, Section 5.8.
/// </summary>
public class CalendarEventChangesResponse : ChangesResponse
{
    // Inherits all properties from ChangesResponse:
    // - AccountId
    // - OldState
    // - NewState
    // - HasMoreChanges
    // - Created (array of ids)
    // - Updated (array of ids)
    // - Destroyed (array of ids)
}
