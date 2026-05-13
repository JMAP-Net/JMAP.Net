using JMAP.Net.Capabilities.Calendars.Methods.Calendar;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Common.Errors;
using JMAP.Net.Persistence.Calendars;

namespace JMAP.Net.Hosting.Engine.Calendar;

/// <summary>
/// Default implementation of Calendar method semantics.
/// </summary>
public sealed class CalendarEngine(ICalendarStore store) : ICalendarEngine
{
    /// <inheritdoc />
    public async ValueTask<CalendarGetResponse> GetAsync(
        JmapExecutionContext context,
        CalendarGetRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);

        EnsureAccountIsAvailable(context, request.AccountId);

        var result = await store.GetAsync(
            request.AccountId,
            request.Ids,
            request.Properties,
            cancellationToken);

        return new CalendarGetResponse
        {
            AccountId = request.AccountId,
            State = result.State,
            List = result.List.ToList(),
            NotFound = result.NotFound.ToList()
        };
    }

    /// <inheritdoc />
    public async ValueTask<CalendarQueryResponse> QueryAsync(
        JmapExecutionContext context,
        CalendarQueryRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);

        EnsureAccountIsAvailable(context, request.AccountId);

        var result = await store.QueryAsync(
            request.AccountId,
            request.Filter,
            request.Sort,
            checked((int)request.Position.Value),
            request.Anchor,
            checked((int)request.AnchorOffset.Value),
            request.Limit is null ? null : checked((uint)request.Limit.Value.Value),
            request.CalculateTotal,
            cancellationToken);

        return new CalendarQueryResponse
        {
            AccountId = request.AccountId,
            QueryState = result.QueryState,
            CanCalculateChanges = result.CanCalculateChanges,
            Position = new JmapUnsignedInt(result.Position),
            Ids = result.Ids.ToList(),
            Total = result.Total is null ? null : new JmapUnsignedInt(result.Total.Value)
        };
    }

    /// <inheritdoc />
    public async ValueTask<CalendarChangesResponse> ChangesAsync(
        JmapExecutionContext context,
        CalendarChangesRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);

        EnsureAccountIsAvailable(context, request.AccountId);

        var result = await store.ChangesAsync(
            request.AccountId,
            request.SinceState,
            request.MaxChanges is null ? null : checked((uint)request.MaxChanges.Value.Value),
            cancellationToken);

        return new CalendarChangesResponse
        {
            AccountId = request.AccountId,
            OldState = result.OldState,
            NewState = result.NewState,
            HasMoreChanges = result.HasMoreChanges,
            Created = result.Created.ToList(),
            Updated = result.Updated.ToList(),
            Destroyed = result.Destroyed.ToList()
        };
    }

    private static void EnsureAccountIsAvailable(JmapExecutionContext context, JmapId accountId)
    {
        if (!context.AccountIds.Contains(accountId))
        {
            throw new JmapMethodException(
                JmapErrorType.AccountNotFound,
                $"The account '{accountId}' was not found.");
        }
    }
}
