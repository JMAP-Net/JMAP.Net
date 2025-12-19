using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.ParticipantIdentity;

/// <summary>
/// Request for ParticipantIdentity/get method.
/// As per JMAP Calendars RFC, Section 3.1.
/// </summary>
public class ParticipantIdentityGetRequest : GetRequest<JMAP.Net.Capabilities.Calendars.Types.ParticipantIdentity>
{
    // Inherits all properties from GetRequest<ParticipantIdentityType>:
    // - AccountId
    // - Ids (nullable to fetch all)
    // - Properties (nullable for all properties)
}
