using JMAP.Net.Capabilities.Core.Methods.Query;

namespace JMAP.Net.Capabilities.Sharing.Methods.Principal;

/// <summary>
/// Request for Principal/queryChanges.
/// As per RFC 9670, Section 2.5.
/// </summary>
public sealed class PrincipalQueryChangesRequest : QueryChangesRequest<PrincipalFilterCondition>
{
}
