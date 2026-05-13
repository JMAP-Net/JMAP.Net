using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Persistence.Stores;

/// <summary>
/// Defines persistence operations for JMAP state-based change calculation.
/// </summary>
public interface IJmapChangesStore
{
    /// <summary>
    /// Calculates object changes since a previous state.
    /// </summary>
    /// <param name="accountId">The JMAP account id.</param>
    /// <param name="sinceState">The state supplied by the client.</param>
    /// <param name="maxChanges">The maximum number of changes to return, or <see langword="null" /> for no requested limit.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The JMAP changes result.</returns>
    ValueTask<JmapChangesResult> ChangesAsync(
        JmapId accountId,
        string sinceState,
        uint? maxChanges,
        CancellationToken cancellationToken = default);
}
