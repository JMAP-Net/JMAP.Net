using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;

/// <summary>
/// Response for CalendarEvent/set method.
/// As per JMAP Calendars RFC, Section 5.9.
/// </summary>
public class CalendarEventSetResponse : SetResponse<JMAP.Net.Capabilities.Calendars.Types.CalendarEvent>
{
    // Inherits all properties from SetResponse<CalendarEventType>:
    // - AccountId
    // - OldState
    // - NewState
    // - Created (map of creation id to CalendarEvent)
    // - Updated (map of id to CalendarEvent or null)
    // - Destroyed (array of ids)
    // - NotCreated (map of creation id to SetError)
    // - NotUpdated (map of id to SetError)
    // - NotDestroyed (map of id to SetError)
}
