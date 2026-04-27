using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Common.Session;

namespace JMAP.Net.Capabilities.Sharing.Types;

/// <summary>
/// Represents an individual, group, location, resource, or other entity used for JMAP Sharing.
/// As per RFC 9670, Section 2.
/// </summary>
public sealed class Principal
{
    /// <summary>
    /// The id of the Principal.
    /// Server-set and immutable.
    /// </summary>
    [JsonPropertyName("id")]
    public required JmapId Id { get; init; }

    /// <summary>
    /// The type of Principal.
    /// </summary>
    [JsonPropertyName("type")]
    public required PrincipalType Type { get; init; }

    /// <summary>
    /// The name of the Principal.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// A longer description of the Principal, or null if no description is available.
    /// </summary>
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; init; }

    /// <summary>
    /// An email address for the Principal, or null if no email is available.
    /// </summary>
    [JsonPropertyName("email")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Email { get; init; }

    /// <summary>
    /// The IANA time zone name for this Principal, if known.
    /// </summary>
    [JsonPropertyName("timeZone")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TimeZone { get; init; }

    /// <summary>
    /// A map of JMAP capability URIs to domain-specific information for this Principal.
    /// </summary>
    [JsonPropertyName("capabilities")]
    public required Dictionary<string, object> Capabilities { get; init; }

    /// <summary>
    /// A map of account id to Account object for each JMAP account containing data for this Principal.
    /// </summary>
    [JsonPropertyName("accounts")]
    public required Dictionary<JmapId, JmapAccount>? Accounts { get; init; }
}
