using JMAP.Net.Capabilities.Calendars;
using JMAP.Net.Capabilities.Core;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Capabilities.Sharing;
using JMAP.Net.Common.Session;
using JMAP.Net.Hosting;
using JMAP.Net.Hosting.Services;

namespace JMAP.Net.TestServer.Infrastructure;

public sealed class TestServerSessionProvider : IJmapSessionProvider
{
    public ValueTask<JmapSession> GetSessionAsync(
        JmapTransaction transaction,
        CancellationToken cancellationToken = default)
    {
        var baseUrl = transaction.HttpContext is null
            ? "http://localhost:5080"
            : $"{transaction.HttpContext.Request.Scheme}://{transaction.HttpContext.Request.Host}";

        var accountId = new JmapId(TestServerData.AccountId);

        return ValueTask.FromResult(new JmapSession
        {
            Capabilities = new Dictionary<string, object>
            {
                [CoreCapability.CapabilityUri] = new Dictionary<string, object>(),
                [PrincipalCapability.CapabilityUri] = new Dictionary<string, object>(),
                [PrincipalOwnerCapability.CapabilityUri] = new Dictionary<string, object>(),
                [CalendarCapability.CapabilityUri] = new Dictionary<string, object>(),
                [CalendarParseCapability.CapabilityUri] = new Dictionary<string, object>(),
                [PrincipalAvailabilityCapability.CapabilityUri] = new Dictionary<string, object>()
            },
            Accounts = new Dictionary<JmapId, JmapAccount>
            {
                [accountId] = new()
                {
                    Name = "Test Account",
                    IsPersonal = true,
                    IsReadOnly = false,
                    AccountCapabilities = new Dictionary<string, object>
                    {
                        [CoreCapability.CapabilityUri] = new Dictionary<string, object>(),
                        [PrincipalCapability.CapabilityUri] = new Dictionary<string, object>(),
                        [PrincipalOwnerCapability.CapabilityUri] = new Dictionary<string, object>(),
                        [CalendarCapability.CapabilityUri] = new Dictionary<string, object>(),
                        [CalendarParseCapability.CapabilityUri] = new Dictionary<string, object>(),
                        [PrincipalAvailabilityCapability.CapabilityUri] = new Dictionary<string, object>()
                    }
                }
            },
            PrimaryAccounts = new Dictionary<string, JmapId>
            {
                [PrincipalCapability.CapabilityUri] = accountId,
                [CalendarCapability.CapabilityUri] = accountId,
                [CalendarParseCapability.CapabilityUri] = accountId,
                [PrincipalAvailabilityCapability.CapabilityUri] = accountId
            },
            Username = "user@example.test",
            ApiUrl = $"{baseUrl}/jmap",
            DownloadUrl = $"{baseUrl}/download/{{accountId}}/{{blobId}}/{{name}}?type={{type}}",
            UploadUrl = $"{baseUrl}/upload/{{accountId}}",
            EventSourceUrl = $"{baseUrl}/events?types={{types}}&closeafter={{closeafter}}&ping={{ping}}",
            State = "test-server-session-1"
        });
    }
}
