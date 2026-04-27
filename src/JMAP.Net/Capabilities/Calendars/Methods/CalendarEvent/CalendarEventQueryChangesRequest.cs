using JMAP.Net.Capabilities.Core.Methods.Query;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;

/// <summary>
/// Request for CalendarEvent/queryChanges method.
/// As per JMAP Calendars RFC, Section 5.12.
/// </summary>
public sealed class CalendarEventQueryChangesRequest : QueryChangesRequest<CalendarEventFilterCondition>
{
}
