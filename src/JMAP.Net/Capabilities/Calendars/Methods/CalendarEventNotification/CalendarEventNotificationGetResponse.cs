using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEventNotification;

/// <summary>
/// Response for CalendarEventNotification/get method.
/// Standard JMAP get response for CalendarEventNotification objects.
/// As per RFC 8984, Section 6.1.
/// </summary>
public class CalendarEventNotificationGetResponse : GetResponse<JMAP.Net.Capabilities.Calendars.Types.CalendarEventNotification>
{
}
