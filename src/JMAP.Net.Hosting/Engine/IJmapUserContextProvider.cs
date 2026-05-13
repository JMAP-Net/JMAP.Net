namespace JMAP.Net.Hosting.Engine;

/// <summary>
/// Resolves the JMAP execution context for the current transaction.
/// </summary>
public interface IJmapUserContextProvider
{
    /// <summary>
    /// Resolves authenticated user and account information for a transaction.
    /// </summary>
    /// <param name="transaction">The active JMAP transaction.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The JMAP execution context.</returns>
    ValueTask<JmapExecutionContext> GetContextAsync(
        JmapTransaction transaction,
        CancellationToken cancellationToken = default);
}
