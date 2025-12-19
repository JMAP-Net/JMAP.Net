using JMAP.Net.Capabilities.Core.Methods.Query;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;

/// <summary>
/// Response for CalendarEvent/queryChanges method.
/// As per JMAP Calendars RFC, Section 5.12.
/// </summary>
public class CalendarEventQueryChangesResponse : QueryChangesResponse
{
    // Inherits all properties from QueryChangesResponse:
    // - AccountId
    // - OldQueryState
    // - NewQueryState
    // - Total (optional)
    // - Removed (array of ids)
    // - Added (array of AddedItem with id and index)
}
