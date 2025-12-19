using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.ParticipantIdentity;

/// <summary>
/// Response for ParticipantIdentity/set method.
/// As per JMAP Calendars RFC, Section 3.3.
/// </summary>
public class ParticipantIdentitySetResponse : SetResponse<JMAP.Net.Capabilities.Calendars.Types.ParticipantIdentity>
{
    // Inherits all properties from SetResponse<ParticipantIdentityType>:
    // - AccountId
    // - OldState
    // - NewState
    // - Created (map of creation id to ParticipantIdentity)
    // - Updated (map of id to ParticipantIdentity or null)
    // - Destroyed (array of ids)
    // - NotCreated (map of creation id to SetError)
    // - NotUpdated (map of id to SetError)
    // - NotDestroyed (map of id to SetError)
}