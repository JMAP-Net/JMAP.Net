using System.Text.Json.Nodes;
using JMAP.Net.Hosting;
using JMAP.Net.Hosting.Services;
using JMAP.Net.TestServer.Infrastructure;

namespace JMAP.Net.TestServer.Handlers;

public sealed class CalendarQueryHandler(TestServerData data) : IJmapMethodHandler
{
    public string MethodName => "Calendar/query";

    public ValueTask<JmapMethodResult> HandleAsync(
        JmapMethodContext context,
        CancellationToken cancellationToken = default)
    {
        var arguments = JsonHandlerSupport.Arguments(context);
        var accountId = JsonHandlerSupport.AccountId(arguments, TestServerData.AccountId);

        return ValueTask.FromResult(new JmapMethodResult
        {
            Name = MethodName,
            Arguments = JsonHandlerSupport.ToArguments(new JsonObject
            {
                ["accountId"] = accountId,
                ["queryState"] = data.CalendarState,
                ["canCalculateChanges"] = true,
                ["position"] = 0,
                ["ids"] = new JsonArray(TestServerData.CalendarId),
                ["total"] = 1
            })
        });
    }
}
