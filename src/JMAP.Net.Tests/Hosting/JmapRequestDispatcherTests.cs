using JMAP.Net.Hosting.Services;
using JMAP.Net.Tests.Hosting.Infrastructure;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Tests.Hosting.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace JMAP.Net.Tests.Hosting;

public class JmapRequestDispatcherTests : JmapHostingTestBase
{
    [Test]
    public async Task Dispatcher_WhenMethodIsUnknown_ShouldReturnUnknownMethodError()
    {
        await using var scope = CreateServiceScope(server => server.AddMethodHandler<CoreEchoHandler>());

        var dispatcher = scope.ServiceProvider.GetRequiredService<IJmapRequestDispatcher>();

        var response = await dispatcher.DispatchAsync(
            CreateTransaction(scope.ServiceProvider),
            CoreRequest(JmapRequestBuilder.Invocation("Unknown/get", "c1")));

        await Assert.That(response.SessionState).IsEqualTo("session-state");
        await Assert.That(response.MethodResponses.Count).IsEqualTo(1);
        using var _ = Assert.Multiple();
        await Assert.That(response.MethodResponses[0].Name).IsEqualTo("error");
        await Assert.That(response.MethodResponses[0].MethodCallId).IsEqualTo("c1");
        await Assert.That(response.MethodResponses[0].Arguments["type"]).IsEqualTo("unknownMethod");
    }

    [Test]
    public async Task Dispatcher_WhenInvocationUsesResultReference_ShouldResolveArgument()
    {
        await using var scope = CreateServiceScope(server => server.AddMethodHandler<CoreEchoHandler>());

        var dispatcher = scope.ServiceProvider.GetRequiredService<IJmapRequestDispatcher>();

        var response = await dispatcher.DispatchAsync(
            CreateTransaction(scope.ServiceProvider),
            CoreRequest(
                JmapRequestBuilder.CoreEcho("c1", "ids", "A"),
                JmapRequestBuilder.Invocation(
                    "Core/echo",
                    "c2",
                    new Dictionary<string, object?>
                    {
                        ["#ids"] = new ResultReference
                        {
                            ResultOf = "c1",
                            Name = "Core/echo",
                            Path = "/ids"
                        }
                    })));

        await Assert.That(response.MethodResponses.Count).IsEqualTo(2);
        using var _ = Assert.Multiple();
        await Assert.That(response.MethodResponses[1].Name).IsEqualTo("Core/echo");
        await Assert.That(response.MethodResponses[1].Arguments.ContainsKey("#ids")).IsFalse();
        await Assert.That(response.MethodResponses[1].Arguments["ids"]).IsEqualTo("A");
    }

    [Test]
    public async Task Dispatcher_WhenResultReferenceCannotBeResolved_ShouldReturnInvalidResultReferenceError()
    {
        await using var scope = CreateServiceScope(server => server.AddMethodHandler<CoreEchoHandler>());

        var dispatcher = scope.ServiceProvider.GetRequiredService<IJmapRequestDispatcher>();

        var response = await dispatcher.DispatchAsync(
            CreateTransaction(scope.ServiceProvider),
            CoreRequest(
                JmapRequestBuilder.Invocation(
                    "Core/echo",
                    "c1",
                    new Dictionary<string, object?>
                    {
                        ["#ids"] = new ResultReference
                        {
                            ResultOf = "missing",
                            Name = "Core/echo",
                            Path = "/ids"
                        }
                    })));

        await Assert.That(response.MethodResponses.Count).IsEqualTo(1);
        using var _ = Assert.Multiple();
        await Assert.That(response.MethodResponses[0].Name).IsEqualTo("error");
        await Assert.That(response.MethodResponses[0].Arguments["type"]).IsEqualTo("invalidResultReference");
    }
}
