using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Sharing.Types;

/// <summary>
/// Records when the user's permissions to access a shared object change.
/// As per RFC 9670, Section 3.
/// </summary>
public sealed class ShareNotification
{
    /// <summary>
    /// The id of the ShareNotification.
    /// Server-set and immutable.
    /// </summary>
    [JsonPropertyName("id")]
    public required JmapId Id { get; init; }

    /// <summary>
    /// The time this notification was created.
    /// Server-set and immutable.
    /// </summary>
    [JsonPropertyName("created")]
    public required JmapUtcDate Created { get; init; }

    /// <summary>
    /// Who made the change.
    /// </summary>
    [JsonPropertyName("changedBy")]
    public required Entity ChangedBy { get; init; }

    /// <summary>
    /// The JMAP data type name for the object whose permissions have changed.
    /// </summary>
    [JsonPropertyName("objectType")]
    public required string ObjectType { get; init; }

    /// <summary>
    /// The id of the account where the shared object exists.
    /// </summary>
    [JsonPropertyName("objectAccountId")]
    public required JmapId ObjectAccountId { get; init; }

    /// <summary>
    /// The id of the object this notification is about.
    /// </summary>
    [JsonPropertyName("objectId")]
    public required JmapId ObjectId { get; init; }

    /// <summary>
    /// The object's myRights value for the user before the change.
    /// </summary>
    [JsonPropertyName("oldRights")]
    public required Dictionary<string, bool>? OldRights { get; init; }

    /// <summary>
    /// The object's myRights value for the user after the change.
    /// </summary>
    [JsonPropertyName("newRights")]
    public required Dictionary<string, bool>? NewRights { get; init; }

    /// <summary>
    /// The name of the shared object at the time the notification was made.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }
}
