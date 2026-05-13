using JMAP.Net.Capabilities.Calendars.Methods.Calendar;
using JMAP.Net.Capabilities.Calendars.Types;
using JMAP.Net.Persistence.Stores;

namespace JMAP.Net.Persistence.Calendars;

/// <summary>
/// Provides persistence operations for JMAP Calendar objects.
/// </summary>
public interface ICalendarStore :
    IJmapGetStore<Calendar>,
    IJmapQueryStore<CalendarFilterCondition>,
    IJmapChangesStore;
