using JMAP.Net.Capabilities.Core.Methods.Query;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEventNotification;

/// <summary>
/// Request for CalendarEventNotification/query method.
/// Standard JMAP query method for searching CalendarEventNotification objects.
/// As per RFC 8984, Section 6.4.
/// </summary>
public class CalendarEventNotificationQueryRequest : QueryRequest<Types.CalendarEventNotificationFilterCondition>
{
}
