using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Persistence.Stores;

/// <summary>
/// Represents the storage result for a JMAP <c>*/changes</c> operation.
/// </summary>
public sealed class JmapChangesResult
{
    /// <summary>
    /// Gets the state from which changes were calculated.
    /// </summary>
    public required string OldState { get; init; }

    /// <summary>
    /// Gets the state after applying the returned changes.
    /// </summary>
    public required string NewState { get; init; }

    /// <summary>
    /// Gets whether more changes are available after <see cref="NewState" />.
    /// </summary>
    public required bool HasMoreChanges { get; init; }

    /// <summary>
    /// Gets ids created since <see cref="OldState" />.
    /// </summary>
    public required IReadOnlyList<JmapId> Created { get; init; }

    /// <summary>
    /// Gets ids updated since <see cref="OldState" />.
    /// </summary>
    public required IReadOnlyList<JmapId> Updated { get; init; }

    /// <summary>
    /// Gets ids destroyed since <see cref="OldState" />.
    /// </summary>
    public required IReadOnlyList<JmapId> Destroyed { get; init; }
}
