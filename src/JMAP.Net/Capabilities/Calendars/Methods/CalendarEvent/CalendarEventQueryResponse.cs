using JMAP.Net.Capabilities.Core.Methods.Query;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;

/// <summary>
/// Response for CalendarEvent/query method.
/// As per JMAP Calendars RFC, Section 5.11.
/// </summary>
public class CalendarEventQueryResponse : QueryResponse
{
    // Inherits all properties from QueryResponse:
    // - AccountId
    // - QueryState
    // - CanCalculateChanges
    // - Position
    // - Ids (array of CalendarEvent ids)
    // - Total (optional)
    // - Limit (optional)
    
    // Note: If expandRecurrences was true, synthetic ids may be returned
    // representing individual instances of recurring events.
}
