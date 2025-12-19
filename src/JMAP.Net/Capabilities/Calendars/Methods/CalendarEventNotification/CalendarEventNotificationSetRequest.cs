using JMAP.Net.Capabilities.Core.Methods;
using JSCalendar.Net;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEventNotification;

/// <summary>
/// Request for CalendarEventNotification/set method.
/// Note: CalendarEventNotifications are server-created and read-only.
/// Only "destroy" is supported; any attempt to create/update 
/// MUST be rejected with a forbidden SetError.
/// As per RFC 8984, Section 6.3.
/// </summary>
public class CalendarEventNotificationSetRequest : SetRequest<JMAP.Net.Capabilities.Calendars.Types.CalendarEventNotification, PatchObject>
{
}
