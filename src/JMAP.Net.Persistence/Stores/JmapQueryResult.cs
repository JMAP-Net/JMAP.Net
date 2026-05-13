using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Persistence.Stores;

/// <summary>
/// Represents the storage result for a JMAP <c>*/query</c> operation.
/// </summary>
public sealed class JmapQueryResult
{
    /// <summary>
    /// Gets the current query state.
    /// </summary>
    public required string QueryState { get; init; }

    /// <summary>
    /// Gets whether the store can calculate query changes for this query shape.
    /// </summary>
    public required bool CanCalculateChanges { get; init; }

    /// <summary>
    /// Gets the zero-based position of the first returned id in the full query result.
    /// </summary>
    public required int Position { get; init; }

    /// <summary>
    /// Gets the returned object ids.
    /// </summary>
    public required IReadOnlyList<JmapId> Ids { get; init; }

    /// <summary>
    /// Gets the total number of results when requested and available.
    /// </summary>
    public int? Total { get; init; }
}
