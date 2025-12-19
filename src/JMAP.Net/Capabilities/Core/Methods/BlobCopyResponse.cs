using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Common.Errors;

namespace JMAP.Net.Capabilities.Core.Methods;

/// <summary>
/// Response for the Blob/copy method.
/// As per RFC 8620, Section 6.3.
/// </summary>
public class BlobCopyResponse
{
    /// <summary>
    /// The id of the account blobs were copied from.
    /// </summary>
    [JsonPropertyName("fromAccountId")]
    public required JmapId FromAccountId { get; init; }

    /// <summary>
    /// The id of the account blobs were copied to.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required JmapId AccountId { get; init; }

    /// <summary>
    /// A map of the blobId in the fromAccount to the id for the blob in the account
    /// it was copied to, or null if none were successfully copied.
    /// </summary>
    [JsonPropertyName("copied")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, JmapId>? Copied { get; init; }

    /// <summary>
    /// A map of blobId to a SetError object for each blob that failed to be copied,
    /// or null if none.
    /// </summary>
    /// <remarks>
    /// The SetError may be any of the standard set errors for create operations.
    /// Additionally, "notFound" may be returned if the blobId cannot be found.
    /// </remarks>
    [JsonPropertyName("notCopied")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, SetError>? NotCopied { get; init; }
}
