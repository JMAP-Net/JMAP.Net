using System.Text.Json;
using JMAP.Net.Hosting.Services;
using JMAP.Net.Tests.Hosting.Infrastructure;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Common.Protocol;
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

    [Test]
    public async Task Dispatcher_WhenResultReferenceUsesEscapedJsonPointerTokens_ShouldResolveArgument()
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
                        ["a/b"] = new Dictionary<string, object?>
                        {
                            ["tilde~key"] = "escaped"
                        }
                    }),
                JmapRequestBuilder.Invocation(
                    "Core/echo",
                    "c2",
                    new Dictionary<string, object?>
                    {
                        ["#value"] = new ResultReference
                        {
                            ResultOf = "c1",
                            Name = "Core/echo",
                            Path = "/a~1b/tilde~0key"
                        }
                    })));

        await Assert.That(response.MethodResponses.Count).IsEqualTo(2);
        await Assert.That(response.MethodResponses[1].Arguments["value"]).IsEqualTo("escaped");
    }

    [Test]
    public async Task Dispatcher_WhenResultReferenceUsesWildcardPath_ShouldResolveArgumentList()
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
                        ["list"] = new List<object?>
                        {
                            new Dictionary<string, object?> { ["id"] = "A" },
                            new Dictionary<string, object?> { ["id"] = "B" }
                        }
                    }),
                JmapRequestBuilder.Invocation(
                    "Core/echo",
                    "c2",
                    new Dictionary<string, object?>
                    {
                        ["#ids"] = new ResultReference
                        {
                            ResultOf = "c1",
                            Name = "Core/echo",
                            Path = "/list/*/id"
                        }
                    })));

        var ids = (IReadOnlyList<object?>)response.MethodResponses[1].Arguments["ids"]!;

        await Assert.That(response.MethodResponses.Count).IsEqualTo(2);
        using var _ = Assert.Multiple();
        await Assert.That(ids.Count).IsEqualTo(2);
        await Assert.That(ids[0]).IsEqualTo("A");
        await Assert.That(ids[1]).IsEqualTo("B");
    }

    [Test]
    public async Task Dispatcher_WhenJsonRequestContainsResultReference_ShouldResolveArgument()
    {
        await using var scope = CreateServiceScope(server => server.AddMethodHandler<CoreEchoHandler>());

        var dispatcher = scope.ServiceProvider.GetRequiredService<IJmapRequestDispatcher>();
        var request = JsonSerializer.Deserialize<JmapRequest>(
            """
            {
              "using": ["urn:ietf:params:jmap:core"],
              "methodCalls": [
                ["Core/echo", { "ids": ["A", "B"] }, "c1"],
                ["Core/echo", { "#ids": { "resultOf": "c1", "name": "Core/echo", "path": "/ids" } }, "c2"]
              ]
            }
            """);

        var response = await dispatcher.DispatchAsync(
            CreateTransaction(scope.ServiceProvider),
            request!);
        var ids = (IReadOnlyList<object?>)response.MethodResponses[1].Arguments["ids"]!;

        await Assert.That(response.MethodResponses.Count).IsEqualTo(2);
        using var _ = Assert.Multiple();
        await Assert.That(ids.Count).IsEqualTo(2);
        await Assert.That(ids[0]).IsEqualTo("A");
        await Assert.That(ids[1]).IsEqualTo("B");
    }
}
