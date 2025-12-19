using JMAP.Net.Capabilities.Core.Methods.Query;

namespace JMAP.Net.Capabilities.Calendars.Methods.Calendar;

/// <summary>
/// Request for Calendar/queryChanges method.
/// Gets the changes to the query results since a previous state.
/// </summary>
public class CalendarQueryChangesRequest : QueryChangesRequest<CalendarFilterCondition>
{
}
