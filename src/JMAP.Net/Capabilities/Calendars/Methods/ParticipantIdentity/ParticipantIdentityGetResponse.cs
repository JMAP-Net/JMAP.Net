using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.ParticipantIdentity;

/// <summary>
/// Response for ParticipantIdentity/get method.
/// As per JMAP Calendars RFC, Section 3.1.
/// </summary>
public class ParticipantIdentityGetResponse : GetResponse<JMAP.Net.Capabilities.Calendars.Types.ParticipantIdentity>
{
    // Inherits all properties from GetResponse<ParticipantIdentityType>:
    // - AccountId
    // - State
    // - List (array of ParticipantIdentity objects)
    // - NotFound (array of ids not found)
}
