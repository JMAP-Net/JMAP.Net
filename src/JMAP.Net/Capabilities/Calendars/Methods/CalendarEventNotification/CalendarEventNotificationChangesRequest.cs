using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEventNotification;

/// <summary>
/// Request for CalendarEventNotification/changes method.
/// Standard JMAP changes method for tracking changes to CalendarEventNotification objects.
/// As per RFC 8984, Section 6.2.
/// </summary>
public class CalendarEventNotificationChangesRequest : ChangesRequest
{
}
