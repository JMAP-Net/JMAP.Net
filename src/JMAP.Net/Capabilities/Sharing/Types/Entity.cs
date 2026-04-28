using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Sharing.Types;

/// <summary>
/// Represents the entity that made a sharing change.
/// As per RFC 9670, Section 3.
/// </summary>
public sealed class Entity
{
    /// <summary>
    /// The name of the entity who made the change.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// The email of the entity who made the change, or null if no email is available.
    /// </summary>
    [JsonPropertyName("email")]
    public required string? Email { get; init; }

    /// <summary>
    /// The id of the Principal corresponding to the entity who made the change, or null if none is associated.
    /// </summary>
    [JsonPropertyName("principalId")]
    public required JmapId? PrincipalId { get; init; }
}
