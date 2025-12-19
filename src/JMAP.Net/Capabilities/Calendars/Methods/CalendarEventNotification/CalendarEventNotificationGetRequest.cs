using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEventNotification;

/// <summary>
/// Request for CalendarEventNotification/get method.
/// Standard JMAP get method for retrieving CalendarEventNotification objects.
/// As per RFC 8984, Section 6.1.
/// </summary>
public class CalendarEventNotificationGetRequest : GetRequest<JMAP.Net.Capabilities.Calendars.Types.CalendarEventNotification>
{
}
