using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Persistence.Stores;

/// <summary>
/// Defines persistence operations for JMAP object retrieval.
/// </summary>
/// <typeparam name="TObject">The JMAP object type returned by the store.</typeparam>
public interface IJmapGetStore<TObject>
{
    /// <summary>
    /// Gets objects by id, or all visible objects when <paramref name="ids" /> is <see langword="null" />.
    /// </summary>
    /// <param name="accountId">The JMAP account id.</param>
    /// <param name="ids">The requested ids, or <see langword="null" /> for all visible records.</param>
    /// <param name="properties">The requested JMAP properties, or <see langword="null" /> for all properties.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The JMAP get result.</returns>
    ValueTask<JmapGetResult<TObject>> GetAsync(
        JmapId accountId,
        IReadOnlyList<JmapId>? ids,
        IReadOnlyList<string>? properties,
        CancellationToken cancellationToken = default);
}
