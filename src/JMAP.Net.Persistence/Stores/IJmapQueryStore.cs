using JMAP.Net.Capabilities.Core.Methods.Query;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Persistence.Stores;

/// <summary>
/// Defines persistence operations for JMAP object queries.
/// </summary>
/// <typeparam name="TFilter">The concrete filter condition type for the queried object type.</typeparam>
public interface IJmapQueryStore<TFilter>
{
    /// <summary>
    /// Queries object ids visible in an account.
    /// </summary>
    /// <param name="accountId">The JMAP account id.</param>
    /// <param name="filter">The filter condition or filter operator tree.</param>
    /// <param name="sort">The requested sort comparators.</param>
    /// <param name="position">The zero-based start position.</param>
    /// <param name="anchor">The anchor id, if any.</param>
    /// <param name="anchorOffset">The offset relative to <paramref name="anchor" />.</param>
    /// <param name="limit">The maximum number of ids to return.</param>
    /// <param name="calculateTotal">Whether the total count should be calculated.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The JMAP query result.</returns>
    ValueTask<JmapQueryResult> QueryAsync(
        JmapId accountId,
        object? filter,
        IReadOnlyList<Comparator>? sort,
        int position,
        JmapId? anchor,
        int anchorOffset,
        uint? limit,
        bool calculateTotal,
        CancellationToken cancellationToken = default);
}
