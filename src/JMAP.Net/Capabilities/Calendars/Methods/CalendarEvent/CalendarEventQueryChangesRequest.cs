using JMAP.Net.Capabilities.Core.Methods.Query;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;

/// <summary>
/// Request for CalendarEvent/queryChanges method.
/// As per JMAP Calendars RFC, Section 5.12.
/// </summary>
public class CalendarEventQueryChangesRequest : QueryChangesRequest<CalendarEventFilterCondition>
{
    // Inherits all properties from QueryChangesRequest<CalendarEventFilterCondition>:
    // - AccountId
    // - Filter
    // - Sort
    // - SinceQueryState
    // - MaxChanges (optional)
    // - UpToId (optional)
    // - CalculateTotal
}
