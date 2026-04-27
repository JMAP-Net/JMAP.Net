using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Calendars.Methods.ParticipantIdentity;

/// <summary>
/// Request for ParticipantIdentity/get method.
/// As per JMAP Calendars RFC, Section 3.1.
/// </summary>
public sealed class ParticipantIdentityGetRequest : GetRequest<JMAP.Net.Capabilities.Calendars.Types.ParticipantIdentity>
{
}
