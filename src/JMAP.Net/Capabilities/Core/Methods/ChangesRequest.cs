using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Core.Methods;

/// <summary>
/// Base request class for Foo/changes methods.
/// As per RFC 8620, Section 5.2.
/// </summary>
public class ChangesRequest
{
    /// <summary>
    /// The id of the account to use.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required JmapId AccountId { get; init; }

    /// <summary>
    /// The current state of the client.
    /// This is the string that was returned as the state argument in the Foo/get response.
    /// </summary>
    [JsonPropertyName("sinceState")]
    public required string SinceState { get; init; }

    /// <summary>
    /// The maximum number of ids to return in the response.
    /// The server MAY choose to return fewer but MUST NOT return more.
    /// If null, the server may choose how many to return.
    /// Must be a positive integer greater than 0.
    /// </summary>
    [JsonPropertyName("maxChanges")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapUnsignedInt? MaxChanges { get; init; }
}
