using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.Calendar;

/// <summary>
/// Response for Calendar/changes method.
/// As per JMAP Calendars RFC, Section 4.2.
/// </summary>
public class CalendarChangesResponse : ChangesResponse
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
