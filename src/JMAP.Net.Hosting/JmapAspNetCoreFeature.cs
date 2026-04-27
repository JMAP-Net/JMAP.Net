namespace JMAP.Net.Hosting;

/// <summary>
/// Exposes the current JMAP transaction to the ASP.NET Core host.
/// </summary>
public sealed class JmapAspNetCoreFeature
{
    /// <summary>
    /// Gets or sets the active JMAP transaction for the current request.
    /// </summary>
    public JmapTransaction? Transaction { get; set; }
}
