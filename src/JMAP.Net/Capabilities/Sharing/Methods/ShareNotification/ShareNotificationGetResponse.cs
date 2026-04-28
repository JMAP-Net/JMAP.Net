using JMAP.Net.Capabilities.Core.Methods;

namespace JMAP.Net.Capabilities.Sharing.Methods.ShareNotification;

/// <summary>
/// Response for ShareNotification/get.
/// As per RFC 9670, Section 3.1.
/// </summary>
public sealed class ShareNotificationGetResponse : GetResponse<Types.ShareNotification>
{
}
