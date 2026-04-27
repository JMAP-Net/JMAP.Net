using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Core.Methods;

/// <summary>
/// Response returned by the JMAP blob upload endpoint.
/// As per RFC 8620, Section 6.1.
/// </summary>
public sealed class BlobUploadResponse
{
    /// <summary>
    /// The id of the account the blob was uploaded to.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required JmapId AccountId { get; init; }

    /// <summary>
    /// The id representing the binary data uploaded.
    /// </summary>
    [JsonPropertyName("blobId")]
    public required JmapId BlobId { get; init; }

    /// <summary>
    /// The media type from the upload request Content-Type header.
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; init; }

    /// <summary>
    /// The size of the uploaded file in octets.
    /// </summary>
    [JsonPropertyName("size")]
    public required JmapUnsignedInt Size { get; init; }
}
