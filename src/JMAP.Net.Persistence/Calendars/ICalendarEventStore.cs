using JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;
using JMAP.Net.Capabilities.Calendars.Types;
using JMAP.Net.Persistence.Stores;

namespace JMAP.Net.Persistence.Calendars;

/// <summary>
/// Provides persistence operations for JMAP CalendarEvent objects.
/// </summary>
public interface ICalendarEventStore :
    IJmapGetStore<CalendarEvent>,
    IJmapQueryStore<CalendarEventFilterCondition>,
    IJmapChangesStore;
