using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.Calendar;

/// <summary>
/// Response for Calendar/copy method.
/// Returns the results of copying calendars between accounts.
/// </summary>
public class CalendarCopyResponse : CopyResponse<JMAP.Net.Capabilities.Calendars.Types.Calendar>
{
    // Inherits all properties from CopyResponse<CalendarType>:
    // - FromAccountId
    // - AccountId
    // - OldState
    // - NewState
    // - Created (map of creation id to Calendar)
    // - NotCreated (map of creation id to SetError)
}
