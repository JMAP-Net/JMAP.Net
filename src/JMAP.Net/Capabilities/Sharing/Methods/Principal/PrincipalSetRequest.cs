using JMAP.Net.Capabilities.Core.Methods;
using JSCalendar.Net;

namespace JMAP.Net.Capabilities.Sharing.Methods.Principal;

/// <summary>
/// Request for Principal/set.
/// As per RFC 9670, Section 2.3.
/// </summary>
public sealed class PrincipalSetRequest : SetRequest<Types.Principal, PatchObject>
{
}
