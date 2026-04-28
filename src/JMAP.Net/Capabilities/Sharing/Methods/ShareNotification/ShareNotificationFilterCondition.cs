using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Sharing.Methods.ShareNotification;

/// <summary>
/// A filter condition for ShareNotification/query.
/// </summary>
public sealed class ShareNotificationFilterCondition
{
    /// <summary>
    /// The creation date must be on or after this date to match.
    /// </summary>
    [JsonPropertyName("after")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapUtcDate? After { get; init; }

    /// <summary>
    /// The creation date must be before this date to match.
    /// </summary>
    [JsonPropertyName("before")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapUtcDate? Before { get; init; }

    /// <summary>
    /// The objectType value must be identical to this value.
    /// </summary>
    [JsonPropertyName("objectType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ObjectType { get; init; }

    /// <summary>
    /// The objectAccountId value must be identical to this value.
    /// </summary>
    [JsonPropertyName("objectAccountId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapId? ObjectAccountId { get; init; }
}
