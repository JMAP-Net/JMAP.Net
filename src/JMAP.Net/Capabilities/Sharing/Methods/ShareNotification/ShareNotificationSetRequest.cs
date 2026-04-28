using JMAP.Net.Capabilities.Core.Methods;
using JSCalendar.Net;

namespace JMAP.Net.Capabilities.Sharing.Methods.ShareNotification;

/// <summary>
/// Request for ShareNotification/set.
/// As per RFC 9670, Section 3.3.
/// </summary>
/// <remarks>
/// RFC 9670 only supports destroy for this method; create and update attempts must be rejected by the server.
/// </remarks>
public sealed class ShareNotificationSetRequest : SetRequest<Types.ShareNotification, PatchObject>
{
}
