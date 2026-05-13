using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Capabilities.Sharing.Types;

namespace JMAP.Net.Persistence.Sharing;

/// <summary>
/// Provides persistence operations for JMAP Principal data.
/// </summary>
public interface IPrincipalStore
{
    /// <summary>
    /// Gets the current state string for Principal data in the account.
    /// </summary>
    /// <param name="accountId">The account id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The Principal state string, or <see langword="null" /> if the account does not exist.</returns>
    ValueTask<string?> GetStateAsync(JmapId accountId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets Principal records by id, or all records when <paramref name="ids" /> is <see langword="null" />.
    /// </summary>
    /// <param name="accountId">The account id.</param>
    /// <param name="ids">The requested ids, or <see langword="null" /> for all records.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The Principal records available to the current user.</returns>
    ValueTask<IReadOnlyList<Principal>> GetAsync(
        JmapId accountId,
        IReadOnlyList<JmapId>? ids,
        CancellationToken cancellationToken = default);
}
