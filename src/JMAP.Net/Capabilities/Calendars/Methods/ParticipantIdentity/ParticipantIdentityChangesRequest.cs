using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.ParticipantIdentity;

/// <summary>
/// Request for ParticipantIdentity/changes method.
/// As per JMAP Calendars RFC, Section 3.2.
/// </summary>
public class ParticipantIdentityChangesRequest : ChangesRequest
{
    // Inherits all properties from ChangesRequest:
    // - AccountId
    // - SinceState
    // - MaxChanges (optional)
}
