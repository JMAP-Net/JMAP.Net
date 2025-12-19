using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Common.Protocol;

/// <summary>
/// Represents a JMAP API response.
/// As per RFC 8620, Section 3.4.
/// </summary>
public class JmapResponse
{
    /// <summary>
    /// An array of responses in the same format as methodCalls on the Request object.
    /// The output of methods MUST be added in the same order that the methods are processed.
    /// </summary>
    [JsonPropertyName("methodResponses")]
    public required List<Invocation> MethodResponses { get; init; }

    /// <summary>
    /// A map of client-specified creation id to server-assigned id.
    /// Only returned if given in the request.
    /// </summary>
    [JsonPropertyName("createdIds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, JmapId>? CreatedIds { get; init; }

    /// <summary>
    /// The current value of the "state" string on the Session object.
    /// Clients may use this to detect if the Session object has changed.
    /// </summary>
    [JsonPropertyName("sessionState")]
    public required string SessionState { get; init; }
}
