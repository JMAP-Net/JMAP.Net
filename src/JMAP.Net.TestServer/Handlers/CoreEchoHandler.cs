using JMAP.Net.Hosting;
using JMAP.Net.Hosting.Services;

namespace JMAP.Net.TestServer.Handlers;

public sealed class CoreEchoHandler : IJmapMethodHandler
{
    public string MethodName => "Core/echo";

    public ValueTask<JmapMethodResult> HandleAsync(
        JmapMethodContext context,
        CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(new JmapMethodResult
        {
            Name = MethodName,
            Arguments = context.Invocation.Arguments
        });
    }
}
