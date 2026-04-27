using JMAP.Net.Common.Session;
using JMAP.Net.Hosting;
using JMAP.Net.Hosting.Services;

namespace JMAP.Net.Tests.Hosting.Infrastructure;

internal sealed class CoreSessionProvider : IJmapSessionProvider
{
    public ValueTask<JmapSession> GetSessionAsync(
        JmapTransaction transaction,
        CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(TestJmapSessions.CoreOnly());
    }
}
