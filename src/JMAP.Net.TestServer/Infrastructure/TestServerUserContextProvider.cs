using System.Security.Claims;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Hosting;
using JMAP.Net.Hosting.Engine;

namespace JMAP.Net.TestServer.Infrastructure;

public sealed class TestServerUserContextProvider : IJmapUserContextProvider
{
    public ValueTask<JmapExecutionContext> GetContextAsync(
        JmapTransaction transaction,
        CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(new JmapExecutionContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity("TestServer")),
            PrincipalId = new JmapId(TestServerData.UserPrincipalId),
            AccountIds = new HashSet<JmapId> { new(TestServerData.AccountId) },
            Transaction = transaction
        });
    }
}
