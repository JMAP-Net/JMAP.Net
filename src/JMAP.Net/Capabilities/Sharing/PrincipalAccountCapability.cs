using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Sharing;

/// <summary>
/// Represents the JMAP Sharing principals account capability.
/// </summary>
public sealed class PrincipalAccountCapability
{
    /// <summary>
    /// The capability URI for the JMAP principals capability.
    /// </summary>
    public const string CapabilityUri = PrincipalCapability.CapabilityUri;

    /// <summary>
    /// The id of the Principal in this account that corresponds to the current user, if any.
    /// </summary>
    [JsonPropertyName("currentUserPrincipalId")]
    public required JmapId? CurrentUserPrincipalId { get; init; }
}
