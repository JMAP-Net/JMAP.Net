using System.Text.Json.Serialization;

namespace JMAP.Net.Common.Session;

/// <summary>
/// Represents a JMAP account - a collection of data.
/// As per RFC 8620, Section 1.6.2.
/// </summary>
public class JmapAccount
{
    /// <summary>
    /// A user-friendly string to show when presenting content from this account.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// True if the account belongs to the authenticated user rather than a group
    /// or shared account.
    /// </summary>
    [JsonPropertyName("isPersonal")]
    public required bool IsPersonal { get; init; }

    /// <summary>
    /// True if the entire account is read-only.
    /// </summary>
    [JsonPropertyName("isReadOnly")]
    public required bool IsReadOnly { get; init; }

    /// <summary>
    /// The set of capability URIs for the methods supported in this account.
    /// Each key is a capability URI, value contains account-specific capability information.
    /// </summary>
    [JsonPropertyName("accountCapabilities")]
    public required Dictionary<string, object> AccountCapabilities { get; init; }
}
