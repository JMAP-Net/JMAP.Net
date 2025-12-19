using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Core.Methods;

/// <summary>
/// Request for the Blob/copy method to copy binary data between accounts.
/// As per RFC 8620, Section 6.3.
/// </summary>
/// <remarks>
/// Binary data may be copied between two different accounts using this method
/// rather than having to download and reupload on the client.
/// </remarks>
public class BlobCopyRequest
{
    /// <summary>
    /// The id of the account to copy blobs from.
    /// </summary>
    [JsonPropertyName("fromAccountId")]
    public required JmapId FromAccountId { get; init; }

    /// <summary>
    /// The id of the account to copy blobs to.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required JmapId AccountId { get; init; }

    /// <summary>
    /// A list of ids of blobs to copy to the other account.
    /// </summary>
    [JsonPropertyName("blobIds")]
    public required List<JmapId> BlobIds { get; init; }
}
