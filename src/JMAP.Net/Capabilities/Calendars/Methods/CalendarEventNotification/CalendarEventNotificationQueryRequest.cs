using JMAP.Net.Capabilities.Core.Methods.Query;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEventNotification;

/// <summary>
/// Request for CalendarEventNotification/query method.
/// Request for CalendarEventNotification/query method.
/// As per RFC 8984, Section 6.4.
/// </summary>
public sealed class CalendarEventNotificationQueryRequest : QueryRequest<Types.CalendarEventNotificationFilterCondition>
{
}
