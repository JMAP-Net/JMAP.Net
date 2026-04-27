using JMAP.Net.Capabilities.Core.Methods;
using JSCalendar.Net;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEventNotification;

/// <summary>
/// Request for CalendarEventNotification/set method.
/// Calendar event notifications are server-created, so this request type mainly exists to
/// model the protocol shape for destroying existing notifications.
/// Servers are expected to reject create and update operations for this method.
/// As per RFC 8984, Section 6.3.
/// </summary>
public sealed class CalendarEventNotificationSetRequest : SetRequest<JMAP.Net.Capabilities.Calendars.Types.CalendarEventNotification, PatchObject>
{
}
