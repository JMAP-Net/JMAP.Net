using JMAP.Net.Common.Protocol;

namespace JMAP.Net.Hosting;

/// <summary>
/// Provides the context available to a JMAP method handler for a single invocation.
/// </summary>
public sealed class JmapMethodContext
{
    /// <summary>
    /// Gets or sets the active transaction.
    /// </summary>
    public required JmapTransaction Transaction { get; init; }

    /// <summary>
    /// Gets or sets the containing JMAP request.
    /// </summary>
    public required JmapRequest Request { get; init; }

    /// <summary>
    /// Gets or sets the invocation currently being handled.
    /// </summary>
    public required Invocation Invocation { get; init; }
}
