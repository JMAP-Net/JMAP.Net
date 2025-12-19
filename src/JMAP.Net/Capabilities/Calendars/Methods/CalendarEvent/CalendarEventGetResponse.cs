using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;

/// <summary>
/// Response for CalendarEvent/get method.
/// As per JMAP Calendars RFC, Section 5.7.
/// </summary>
public class CalendarEventGetResponse : GetResponse<JMAP.Net.Capabilities.Calendars.Types.CalendarEvent>
{
    // Inherits all properties from GetResponse<CalendarEventType>:
    // - AccountId
    // - State
    // - List (array of CalendarEvent objects)
    // - NotFound (array of ids not found)
}
