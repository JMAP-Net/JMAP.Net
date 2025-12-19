using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.Calendar;

/// <summary>
/// Request for Calendar/copy method.
/// Copies calendars from one account to another.
/// Useful for multi-account scenarios.
/// </summary>
public class CalendarCopyRequest : CopyRequest<JMAP.Net.Capabilities.Calendars.Types.Calendar>
{
    // Inherits all properties from CopyRequest<CalendarType>:
    // - FromAccountId
    // - IfFromInState
    // - AccountId
    // - IfInState
    // - Create (map of creation id to Calendar with id to copy)
    // - OnSuccessDestroyOriginal
    // - DestroyFromIfInState
}
