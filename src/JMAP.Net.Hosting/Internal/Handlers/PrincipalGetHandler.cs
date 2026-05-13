using System.Text.Json;
using JMAP.Net.Capabilities.Sharing.Methods.Principal;
using JMAP.Net.Hosting.Engine;
using JMAP.Net.Hosting.Engine.Principal;
using JMAP.Net.Hosting.Services;

namespace JMAP.Net.Hosting.Internal.Handlers;

internal sealed class PrincipalGetHandler(
    IJmapUserContextProvider userContextProvider,
    IPrincipalEngine principalEngine)
    : IJmapMethodHandler
{
    public string MethodName => "Principal/get";

    public async ValueTask<JmapMethodResult> HandleAsync(
        JmapMethodContext context,
        CancellationToken cancellationToken = default)
    {
        var request = DeserializeArguments<PrincipalGetRequest>(context.Invocation.Arguments);
        var executionContext = await userContextProvider.GetContextAsync(context.Transaction, cancellationToken);
        var response = await principalEngine.GetAsync(executionContext, request, cancellationToken);

        return new JmapMethodResult
        {
            Name = MethodName,
            Arguments = ToArguments(response)
        };
    }

    private static T DeserializeArguments<T>(Dictionary<string, object?> arguments)
    {
        var json = JsonSerializer.Serialize(arguments);
        return JsonSerializer.Deserialize<T>(json)
            ?? throw new JsonException($"Could not deserialize arguments as {typeof(T).Name}.");
    }

    private static Dictionary<string, object?> ToArguments<T>(T value)
    {
        var json = JsonSerializer.Serialize(value);
        return JsonSerializer.Deserialize<Dictionary<string, object?>>(json)
            ?? throw new JsonException($"Could not serialize {typeof(T).Name} as JMAP method arguments.");
    }
}
