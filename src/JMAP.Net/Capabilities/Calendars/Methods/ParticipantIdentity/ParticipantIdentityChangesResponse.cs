using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.ParticipantIdentity;

/// <summary>
/// Response for ParticipantIdentity/changes method.
/// As per JMAP Calendars RFC, Section 3.2.
/// </summary>
public class ParticipantIdentityChangesResponse : ChangesResponse
{
    // Inherits all properties from ChangesResponse:
    // - AccountId
    // - OldState
    // - NewState
    // - HasMoreChanges
    // - Created (array of ids)
    // - Updated (array of ids)
    // - Destroyed (array of ids)
}
