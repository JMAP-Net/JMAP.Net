using System.Collections.Concurrent;
using JMAP.Net.Common.Session;
using Microsoft.AspNetCore.Http;

namespace JMAP.Net.Hosting;

/// <summary>
/// Represents the request-scoped state associated with a JMAP host request.
/// </summary>
public sealed class JmapTransaction
{
    /// <summary>
    /// Gets or sets the request service provider.
    /// </summary>
    public required IServiceProvider Services { get; init; }

    /// <summary>
    /// Gets or sets the current HTTP context when running under ASP.NET Core.
    /// </summary>
    public HttpContext? HttpContext { get; init; }

    /// <summary>
    /// Gets or sets the resolved JMAP session for the current request.
    /// </summary>
    public JmapSession? Session { get; set; }

    /// <summary>
    /// Gets a property bag for host- or application-specific request state.
    /// </summary>
    public ConcurrentDictionary<string, object?> Properties { get; } = new(StringComparer.Ordinal);
}
