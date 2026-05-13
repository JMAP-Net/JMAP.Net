using JMAP.Net.Capabilities.Calendars.Methods.Calendar;
using JMAP.Net.Capabilities.Calendars.Types;
using JMAP.Net.Capabilities.Core.Methods.Query;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Persistence.Calendars;
using JMAP.Net.Persistence.Stores;

namespace JMAP.Net.TestServer.Infrastructure;

public sealed class InMemoryCalendarStore(TestServerData data) : ICalendarStore
{
    public ValueTask<JmapGetResult<Calendar>> GetAsync(
        JmapId accountId,
        IReadOnlyList<JmapId>? ids,
        IReadOnlyList<string>? properties,
        CancellationToken cancellationToken = default)
    {
        var calendarId = new JmapId(TestServerData.CalendarId);
        var list = ids is null || ids.Contains(calendarId)
            ? new[] { data.WorkCalendar }.ToList()
            : [];

        var found = list.Select(calendar => calendar.Id).ToHashSet();
        var notFound = ids is null
            ? []
            : ids.Where(id => !found.Contains(id)).ToList();

        return ValueTask.FromResult(new JmapGetResult<Calendar>
        {
            State = data.CalendarState,
            List = list,
            NotFound = notFound
        });
    }

    public ValueTask<JmapQueryResult> QueryAsync(
        JmapId accountId,
        object? filter,
        IReadOnlyList<Comparator>? sort,
        int position,
        JmapId? anchor,
        int anchorOffset,
        uint? limit,
        bool calculateTotal,
        CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(new JmapQueryResult
        {
            QueryState = data.CalendarState,
            CanCalculateChanges = true,
            Position = 0,
            Ids = [new JmapId(TestServerData.CalendarId)],
            Total = 1
        });
    }

    public ValueTask<JmapChangesResult> ChangesAsync(
        JmapId accountId,
        string sinceState,
        uint? maxChanges,
        CancellationToken cancellationToken = default)
    {
        var hasChanges = sinceState == data.CalendarState0;

        return ValueTask.FromResult(new JmapChangesResult
        {
            OldState = sinceState,
            NewState = data.CalendarState,
            HasMoreChanges = false,
            Created = hasChanges ? [new JmapId(TestServerData.CalendarId)] : [],
            Updated = [],
            Destroyed = []
        });
    }
}
