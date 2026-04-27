namespace JMAP.Net.Hosting.Services;

/// <summary>
/// Defines a handler that can process a specific JMAP method name.
/// </summary>
public interface IJmapMethodHandler
{
    /// <summary>
    /// Gets the method name handled by this instance.
    /// </summary>
    string MethodName { get; }

    /// <summary>
    /// Handles a single JMAP method invocation.
    /// </summary>
    /// <param name="context">The method context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The method result.</returns>
    ValueTask<JmapMethodResult> HandleAsync(JmapMethodContext context, CancellationToken cancellationToken = default);
}
