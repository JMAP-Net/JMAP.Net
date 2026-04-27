using JMAP.Net.Hosting;
using JMAP.Net.Hosting.Services;

namespace JMAP.Net.Tests.Hosting.Handlers;

internal sealed class DelayedReadHandler(DispatchConcurrencyProbe probe)
    : IJmapMethodHandler, IJmapMethodExecutionMetadata
{
    public string MethodName => "Test/read";

    public JmapMethodExecutionMode ExecutionMode => JmapMethodExecutionMode.ParallelRead;

    public string? GetConcurrencyKey(JmapMethodContext context) => null;

    public async ValueTask<JmapMethodResult> HandleAsync(
        JmapMethodContext context,
        CancellationToken cancellationToken = default)
    {
        await probe.RunAsync(GetDelayMilliseconds(context), cancellationToken);

        return new JmapMethodResult
        {
            Name = MethodName,
            Arguments = context.Invocation.Arguments
        };
    }

    private static int GetDelayMilliseconds(JmapMethodContext context)
    {
        return context.Invocation.Arguments.TryGetValue("delayMilliseconds", out var delayMilliseconds)
            && delayMilliseconds is int value
                ? value
                : 1;
    }
}
