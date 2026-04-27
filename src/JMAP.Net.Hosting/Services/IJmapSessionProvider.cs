using JMAP.Net.Common.Session;

namespace JMAP.Net.Hosting.Services;

/// <summary>
/// Provides the JMAP session document for the current request.
/// </summary>
public interface IJmapSessionProvider
{
    /// <summary>
    /// Resolves the JMAP session for the active request.
    /// </summary>
    /// <param name="transaction">The active JMAP transaction.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The resolved JMAP session.</returns>
    ValueTask<JmapSession> GetSessionAsync(JmapTransaction transaction, CancellationToken cancellationToken = default);
}
