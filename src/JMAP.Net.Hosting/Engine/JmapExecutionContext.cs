using System.Security.Claims;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Hosting.Engine;

/// <summary>
/// Provides authenticated user and account information for JMAP method execution.
/// </summary>
public sealed class JmapExecutionContext
{
    /// <summary>
    /// Gets the authenticated user principal.
    /// </summary>
    public required ClaimsPrincipal User { get; init; }

    /// <summary>
    /// Gets the JMAP Principal id associated with the authenticated user.
    /// </summary>
    public required JmapId PrincipalId { get; init; }

    /// <summary>
    /// Gets the account ids the authenticated user may access.
    /// </summary>
    public required IReadOnlySet<JmapId> AccountIds { get; init; }

    /// <summary>
    /// Gets the active JMAP transaction.
    /// </summary>
    public required JmapTransaction Transaction { get; init; }
}
