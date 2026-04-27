using JMAP.Net.Capabilities.Core;
using JMAP.Net.Common.Session;

namespace JMAP.Net.Tests.Hosting.Infrastructure;

internal static class TestJmapSessions
{
    public static JmapSession CoreOnly(string state = "session-state")
    {
        return new JmapSession
        {
            Capabilities = new Dictionary<string, object>
            {
                [CoreCapability.CapabilityUri] = new Dictionary<string, object>()
            },
            Accounts = new Dictionary<JMAP.Net.Capabilities.Core.Types.JmapId, JmapAccount>(),
            PrimaryAccounts = new Dictionary<string, JMAP.Net.Capabilities.Core.Types.JmapId>(),
            Username = "user@example.com",
            ApiUrl = "https://example.test/jmap",
            DownloadUrl = "https://example.test/download/{accountId}/{blobId}/{name}?type={type}",
            UploadUrl = "https://example.test/upload/{accountId}",
            EventSourceUrl = "https://example.test/events?types={types}&closeafter={closeafter}&ping={ping}",
            State = state
        };
    }
}
