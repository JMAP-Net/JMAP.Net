using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;

/// <summary>
/// Response for CalendarEvent/copy method.
/// As per JMAP Calendars RFC, Section 5.10.
/// </summary>
public class CalendarEventCopyResponse : CopyResponse<JMAP.Net.Capabilities.Calendars.Types.CalendarEvent>
{
    // Inherits all properties from CopyResponse<CalendarEventType>:
    // - FromAccountId
    // - AccountId
    // - OldState
    // - NewState
    // - Created (map of creation id to CalendarEvent)
    // - NotCreated (map of creation id to SetError)
}
