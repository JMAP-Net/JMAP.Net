using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Sharing.Methods.ShareNotification;

/// <summary>
/// Request for ShareNotification/get.
/// As per RFC 9670, Section 3.1.
/// </summary>
public sealed class ShareNotificationGetRequest : GetRequest<Types.ShareNotification>
{
}
