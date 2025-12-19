using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Core;

/// <summary>
/// Represents the core JMAP capability as defined in RFC 8620, Section 2.
/// URN: urn:ietf:params:jmap:core
/// </summary>
public class CoreCapability
{
    public const string CapabilityUri = "urn:ietf:params:jmap:core";

    /// <summary>
    /// The maximum file size, in octets, that the server will accept for a single file upload.
    /// Suggested minimum: 50,000,000.
    /// </summary>
    [JsonPropertyName("maxSizeUpload")]
    public required JmapUnsignedInt MaxSizeUpload { get; init; }

    /// <summary>
    /// The maximum number of concurrent requests the server will accept to the upload endpoint.
    /// Suggested minimum: 4.
    /// </summary>
    [JsonPropertyName("maxConcurrentUpload")]
    public required JmapUnsignedInt MaxConcurrentUpload { get; init; }

    /// <summary>
    /// The maximum size, in octets, that the server will accept for a single request to the API endpoint.
    /// Suggested minimum: 10,000,000.
    /// </summary>
    [JsonPropertyName("maxSizeRequest")]
    public required JmapUnsignedInt MaxSizeRequest { get; init; }

    /// <summary>
    /// The maximum number of concurrent requests the server will accept to the API endpoint.
    /// Suggested minimum: 4.
    /// </summary>
    [JsonPropertyName("maxConcurrentRequests")]
    public required JmapUnsignedInt MaxConcurrentRequests { get; init; }

    /// <summary>
    /// The maximum number of method calls the server will accept in a single request.
    /// Suggested minimum: 16.
    /// </summary>
    [JsonPropertyName("maxCallsInRequest")]
    public required JmapUnsignedInt MaxCallsInRequest { get; init; }

    /// <summary>
    /// The maximum number of objects that the client may request in a single /get method call.
    /// Suggested minimum: 500.
    /// </summary>
    [JsonPropertyName("maxObjectsInGet")]
    public required JmapUnsignedInt MaxObjectsInGet { get; init; }

    /// <summary>
    /// The maximum number of objects the client may send to create, update, or destroy
    /// in a single /set method call.
    /// Suggested minimum: 500.
    /// </summary>
    [JsonPropertyName("maxObjectsInSet")]
    public required JmapUnsignedInt MaxObjectsInSet { get; init; }

    /// <summary>
    /// A list of collation algorithm identifiers (as per RFC 4790) that the server
    /// supports for sorting when querying records.
    /// </summary>
    [JsonPropertyName("collationAlgorithms")]
    public required List<string> CollationAlgorithms { get; init; }
}
