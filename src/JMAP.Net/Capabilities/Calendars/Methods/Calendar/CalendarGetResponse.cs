using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.Calendar;

/// <summary>
/// Response for Calendar/get method.
/// As per JMAP Calendars RFC, Section 4.1.
/// </summary>
public class CalendarGetResponse : GetResponse<JMAP.Net.Capabilities.Calendars.Types.Calendar>
{
    // Inherits all properties from GetResponse<CalendarType>:
    // - AccountId
    // - State
    // - List (array of Calendar objects)
    // - NotFound (array of ids not found)
}
