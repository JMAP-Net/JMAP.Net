using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Core.Methods;

/// <summary>
/// Represents the values used to expand a JMAP blob download URL template.
/// As per RFC 8620, Section 6.2.
/// </summary>
public sealed class BlobDownloadRequest
{
    /// <summary>
    /// The id of the account the blob belongs to.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required JmapId AccountId { get; init; }

    /// <summary>
    /// The id representing the binary data to download.
    /// </summary>
    [JsonPropertyName("blobId")]
    public required JmapId BlobId { get; init; }

    /// <summary>
    /// The media type the server should use for the response Content-Type header.
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; init; }

    /// <summary>
    /// The name the server should use for the file in the response Content-Disposition header.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }
}
