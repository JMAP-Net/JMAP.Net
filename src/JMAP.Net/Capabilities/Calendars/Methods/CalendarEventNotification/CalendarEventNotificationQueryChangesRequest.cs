using JMAP.Net.Capabilities.Core.Methods.Query;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEventNotification;

/// <summary>
/// Request for CalendarEventNotification/queryChanges method.
/// Request for CalendarEventNotification/queryChanges method.
/// As per RFC 8984, Section 6.5.
/// </summary>
public sealed class CalendarEventNotificationQueryChangesRequest : QueryChangesRequest<Types.CalendarEventNotificationFilterCondition>
{
}
