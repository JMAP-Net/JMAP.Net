namespace JMAP.Net.Hosting.Services;

/// <summary>
/// Provides optional scheduling metadata for JMAP method handlers.
/// </summary>
public interface IJmapMethodExecutionMetadata
{
    /// <summary>
    /// Gets the execution mode used by the request dispatcher.
    /// </summary>
    JmapMethodExecutionMode ExecutionMode { get; }

    /// <summary>
    /// Gets the key used to avoid unsafe parallel execution with related method calls.
    /// </summary>
    /// <param name="context">The method context.</param>
    /// <returns>
    /// A stable key for the affected account and resource type, or <see langword="null" />
    /// when the method has no shared state dependency.
    /// </returns>
    string? GetConcurrencyKey(JmapMethodContext context);
}
