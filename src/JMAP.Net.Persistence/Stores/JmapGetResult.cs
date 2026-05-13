using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Persistence.Stores;

/// <summary>
/// Represents the storage result for a JMAP <c>*/get</c> operation.
/// </summary>
/// <typeparam name="TObject">The JMAP object type returned by the store.</typeparam>
public sealed class JmapGetResult<TObject>
{
    /// <summary>
    /// Gets the account state for this object type.
    /// </summary>
    public required string State { get; init; }

    /// <summary>
    /// Gets the objects found by the store.
    /// </summary>
    public required IReadOnlyList<TObject> List { get; init; }

    /// <summary>
    /// Gets the requested ids that were not found or not visible to the caller.
    /// </summary>
    public required IReadOnlyList<JmapId> NotFound { get; init; }
}
