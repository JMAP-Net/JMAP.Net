using JMAP.Net.Common.Protocol;

namespace JMAP.Net.Hosting.Services;

/// <summary>
/// Dispatches an incoming JMAP request to the registered method handlers.
/// </summary>
public interface IJmapRequestDispatcher
{
    /// <summary>
    /// Dispatches the specified JMAP request.
    /// </summary>
    /// <param name="transaction">The active JMAP transaction.</param>
    /// <param name="request">The incoming request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The JMAP response envelope.</returns>
    ValueTask<JmapResponse> DispatchAsync(
        JmapTransaction transaction,
        JmapRequest request,
        CancellationToken cancellationToken = default);
}
