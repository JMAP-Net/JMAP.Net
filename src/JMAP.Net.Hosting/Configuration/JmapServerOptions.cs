namespace JMAP.Net.Hosting.Configuration;

/// <summary>
/// Provides the core options used by the JMAP server services.
/// </summary>
public sealed class JmapServerOptions
{
    /// <summary>
    /// Gets or sets the absolute API path that accepts JMAP method calls.
    /// </summary>
    public string ApiPath { get; set; } = "/jmap";

    /// <summary>
    /// Gets or sets the absolute path that serves the JMAP session document.
    /// </summary>
    public string SessionPath { get; set; } = "/.well-known/jmap";

    /// <summary>
    /// Gets or sets a value indicating whether independent JMAP method calls may be dispatched in parallel.
    /// </summary>
    public bool EnableParallelMethodDispatch { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of method calls that may run at the same time.
    /// </summary>
    public int MaxParallelMethodCalls { get; set; } = Environment.ProcessorCount;
}
