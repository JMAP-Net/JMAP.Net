using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.Calendar;

/// <summary>
/// Response for Calendar/set method.
/// As per JMAP Calendars RFC, Section 4.3.
/// </summary>
public class CalendarSetResponse : SetResponse<JMAP.Net.Capabilities.Calendars.Types.Calendar>
{
    // Inherits all properties from SetResponse<CalendarType>:
    // - AccountId
    // - OldState
    // - NewState
    // - Created (map of creation id to Calendar)
    // - Updated (map of id to Calendar or null)
    // - Destroyed (array of ids)
    // - NotCreated (map of creation id to SetError)
    // - NotUpdated (map of id to SetError)
    // - NotDestroyed (map of id to SetError)
}
