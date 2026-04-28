using JMAP.Net.Capabilities.Core.Methods.Query;

namespace JMAP.Net.Capabilities.Sharing.Methods.ShareNotification;

/// <summary>
/// Request for ShareNotification/queryChanges.
/// As per RFC 9670, Section 3.5.
/// </summary>
public sealed class ShareNotificationQueryChangesRequest : QueryChangesRequest<ShareNotificationFilterCondition>
{
}
