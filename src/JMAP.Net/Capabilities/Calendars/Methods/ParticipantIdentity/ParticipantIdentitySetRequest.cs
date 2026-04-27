using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Capabilities.Core.Methods;
using JSCalendar.Net;

namespace JMAP.Net.Capabilities.Calendars.Methods.ParticipantIdentity;

/// <summary>
/// Request for ParticipantIdentity/set method.
/// As per JMAP Calendars RFC, Section 3.3.
/// </summary>
public sealed class ParticipantIdentitySetRequest : SetRequest<JMAP.Net.Capabilities.Calendars.Types.ParticipantIdentity, PatchObject>
{
    /// <summary>
    /// If set, and all creates, updates, and destroys succeed, the server attempts to make
    /// the referenced identity the account's default participant identity.
    /// </summary>
    [JsonPropertyName("onSuccessSetIsDefault")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapId? OnSuccessSetIsDefault { get; init; }
}
