using JMAP.Net.Capabilities.Core.Methods.Query;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;

/// <summary>
/// Response for CalendarEvent/query method.
/// As per JMAP Calendars RFC, Section 5.11.
/// </summary>
public sealed class CalendarEventQueryResponse : QueryResponse
{
    // If expandRecurrences was true, synthetic ids may be returned
    // for individual instances of recurring events.
}
