using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Calendars.Types;

/// <summary>
/// A ParticipantIdentity stores information about a URI that represents the user
/// within that account in an event's participants.
/// As per JMAP Calendars RFC, Section 3.
/// </summary>
public class ParticipantIdentity
{
    /// <summary>
    /// The id of the ParticipantIdentity.
    /// Immutable; server-set.
    /// </summary>
    [JsonPropertyName("id")]
    public required JmapId Id { get; init; }

    /// <summary>
    /// The display name of the participant to use when adding this participant to an event.
    /// Default: "".
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Name { get; init; } = "";

    /// <summary>
    /// The URI that represents this participant for scheduling.
    /// This URI may also be the URI for one of the sendTo methods.
    /// </summary>
    [JsonPropertyName("scheduleId")]
    public required string ScheduleId { get; init; }

    /// <summary>
    /// Represents methods by which the participant may receive invitations and updates.
    /// The keys are the available methods (ASCII alphanumeric only).
    /// The values are URIs for the method specified in the key.
    /// </summary>
    [JsonPropertyName("sendTo")]
    public required Dictionary<string, string> SendTo { get; init; }

    /// <summary>
    /// Server-set. Should be true for exactly one participant identity in any account.
    /// The default identity should be used by clients when they need to choose
    /// an identity for the user.
    /// </summary>
    [JsonPropertyName("isDefault")]
    public required bool IsDefault { get; init; }
}
