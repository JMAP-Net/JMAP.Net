using JMAP.Net.Capabilities.Core.Methods.Query;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEventNotification;

/// <summary>
/// Request for CalendarEventNotification/queryChanges method.
/// Standard JMAP queryChanges method for tracking changes to query results.
/// As per RFC 8984, Section 6.5.
/// </summary>
public class CalendarEventNotificationQueryChangesRequest : QueryChangesRequest<Types.CalendarEventNotificationFilterCondition>
{
}
