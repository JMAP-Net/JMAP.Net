using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.Calendar;

/// <summary>
/// Request for Calendar/copy method.
/// Copies calendar records from one account to another.
/// As per JMAP Calendars RFC, Section 4.4.
/// </summary>
public class CalendarCopyRequest : CopyRequest<JMAP.Net.Capabilities.Calendars.Types.Calendar>
{
}
