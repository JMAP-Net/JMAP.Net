namespace JMAP.Net.Hosting.Configuration;

/// <summary>
/// Provides the ASP.NET Core specific options used by the JMAP hosting integration.
/// </summary>
public sealed class JmapAspNetCoreOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether non-HTTPS requests should be rejected.
    /// </summary>
    public bool RequireHttps { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether JSON responses should be indented.
    /// </summary>
    public bool WriteIndentedResponses { get; set; }
}
