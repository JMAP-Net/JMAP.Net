using JMAP.Net.Hosting;
using JMAP.Net.Hosting.Services;

namespace JMAP.Net.Tests.Hosting.Handlers;

internal sealed class CoreEchoHandler : IJmapMethodHandler
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
