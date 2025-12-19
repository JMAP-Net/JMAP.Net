using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Common.Protocol;

/// <summary>
/// Represents a JMAP API request.
/// As per RFC 8620, Section 3.3.
/// </summary>
public class JmapRequest
{
    /// <summary>
    /// The set of capabilities the client wishes to use.
    /// The client MAY include capability identifiers even if the method calls
    /// it makes do not utilize those capabilities.
    /// </summary>
    [JsonPropertyName("using")]
    public required List<string> Using { get; init; }

    /// <summary>
    /// An array of method calls to process on the server.
    /// The method calls MUST be processed sequentially, in order.
    /// </summary>
    [JsonPropertyName("methodCalls")]
    public required List<Invocation> MethodCalls { get; init; }

    /// <summary>
    /// A map of a client-specified creation id to the id the server assigned
    /// when a record was successfully created.
    /// Optional - may be null or omitted.
    /// </summary>
    [JsonPropertyName("createdIds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, JmapId>? CreatedIds { get; init; }
}
