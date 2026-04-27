using JMAP.Net.Capabilities.Core.Methods.Query;

namespace JMAP.Net.Capabilities.Sharing.Methods.Principal;

/// <summary>
/// Request for Principal/query.
/// As per RFC 9670, Section 2.4.
/// </summary>
public sealed class PrincipalQueryRequest : QueryRequest<PrincipalFilterCondition>
{
}
