using JMAP.Net.Hosting.Services;
using JMAP.Net.Tests.Hosting.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace JMAP.Net.Tests.Hosting;

public class JmapRequestDispatcherTests : JmapHostingTestBase
{
    [Test]
    public async Task Dispatcher_WhenMethodIsUnknown_ShouldReturnUnknownMethodError()
    {
        await using var scope = CreateServiceScope();

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
}
