using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Sharing;

/// <summary>
/// Represents the JMAP Sharing principals owner account capability.
/// URN: urn:ietf:params:jmap:principals:owner
/// </summary>
public sealed class PrincipalOwnerCapability
{
    /// <summary>
    /// The capability URI for the JMAP principals owner capability.
    /// </summary>
    public const string CapabilityUri = "urn:ietf:params:jmap:principals:owner";

    /// <summary>
    /// The id of an account with the principals capability containing the corresponding Principal object.
    /// </summary>
    [JsonPropertyName("accountIdForPrincipal")]
    public required JmapId AccountIdForPrincipal { get; init; }

    /// <summary>
    /// The id of the Principal that owns this account.
    /// </summary>
    [JsonPropertyName("principalId")]
    public required JmapId PrincipalId { get; init; }
}
