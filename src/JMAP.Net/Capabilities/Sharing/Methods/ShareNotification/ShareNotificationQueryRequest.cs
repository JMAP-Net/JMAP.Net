using JMAP.Net.Capabilities.Core.Methods.Query;

namespace JMAP.Net.Capabilities.Sharing.Methods.ShareNotification;

/// <summary>
/// Request for ShareNotification/query.
/// As per RFC 9670, Section 3.4.
/// </summary>
public sealed class ShareNotificationQueryRequest : QueryRequest<ShareNotificationFilterCondition>
{
}
