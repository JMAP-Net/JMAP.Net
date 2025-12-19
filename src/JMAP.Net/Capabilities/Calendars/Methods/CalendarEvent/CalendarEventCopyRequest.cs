using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;

/// <summary>
/// Request for CalendarEvent/copy method.
/// As per JMAP Calendars RFC, Section 5.10.
/// </summary>
public class CalendarEventCopyRequest : CopyRequest<JMAP.Net.Capabilities.Calendars.Types.CalendarEvent>
{
    // Inherits all properties from CopyRequest<CalendarEventType>:
    // - FromAccountId
    // - IfFromInState
    // - AccountId
    // - IfInState
    // - Create (map of creation id to CalendarEvent with id to copy)
    // - OnSuccessDestroyOriginal
    // - DestroyFromIfInState
}
