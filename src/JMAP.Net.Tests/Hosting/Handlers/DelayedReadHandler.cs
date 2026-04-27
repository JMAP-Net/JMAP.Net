using JMAP.Net.Hosting;
using JMAP.Net.Hosting.Services;
using System.Text.Json;

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
            ? delayMilliseconds switch
            {
                int value => value,
                JsonElement { ValueKind: JsonValueKind.Number } value when value.TryGetInt32(out var result) => result,
                _ => 1
            }
            : 1;
    }
}
