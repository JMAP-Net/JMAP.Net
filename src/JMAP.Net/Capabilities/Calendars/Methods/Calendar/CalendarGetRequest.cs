using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.Calendar;

/// <summary>
/// Request for Calendar/get method.
/// As per JMAP Calendars RFC, Section 4.1.
/// </summary>
public class CalendarGetRequest : GetRequest<JMAP.Net.Capabilities.Calendars.Types.Calendar>
{
    // Inherits all properties from GetRequest<CalendarType>:
    // - AccountId
    // - Ids (nullable for fetching all)
    // - Properties (nullable for all properties)
}
