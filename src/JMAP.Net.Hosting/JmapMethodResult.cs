namespace JMAP.Net.Hosting;

/// <summary>
/// Represents the result returned by a JMAP method handler.
/// </summary>
public sealed class JmapMethodResult
{
    /// <summary>
    /// Gets or sets the response method name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets or sets the response arguments.
    /// </summary>
    public required Dictionary<string, object?> Arguments { get; init; }
}
