using JMAP.Net.Capabilities.Core.Methods.Query;

namespace JMAP.Net.Capabilities.Calendars.Methods.Calendar;

/// <summary>
/// Request for Calendar/query method.
/// Searches for calendars matching the given filter criteria.
/// </summary>
public class CalendarQueryRequest : QueryRequest<CalendarFilterCondition>
{
}
