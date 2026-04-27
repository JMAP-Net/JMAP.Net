using JMAP.Net.Hosting.Services;
using JMAP.Net.Tests.Hosting.Handlers;
using JMAP.Net.Tests.Hosting.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace JMAP.Net.Tests.Hosting;

public class JmapRequestDispatcherParallelTests : JmapHostingTestBase
{
    [Test]
    public async Task Dispatcher_WhenParallelDispatchIsEnabled_ShouldRunCompatibleReadsConcurrently()
    {
        var probe = new DispatchConcurrencyProbe();
        await using var scope = CreateParallelScope(
            probe,
            builder => builder.AddMethodHandler<DelayedReadHandler>());

        var dispatcher = scope.ServiceProvider.GetRequiredService<IJmapRequestDispatcher>();

        var response = await dispatcher.DispatchAsync(
            CreateTransaction(scope.ServiceProvider),
            CoreRequest(
                JmapRequestBuilder.DelayedRead("c1", 200),
                JmapRequestBuilder.DelayedRead("c2", 20)));

        await Assert.That(probe.MaxObservedConcurrency).IsGreaterThanOrEqualTo(2);
        using var _ = Assert.Multiple();
        await Assert.That(response.MethodResponses[0].MethodCallId).IsEqualTo("c1");
        await Assert.That(response.MethodResponses[1].MethodCallId).IsEqualTo("c2");
    }

    [Test]
    public async Task Dispatcher_WhenExclusiveWritesShareConcurrencyKey_ShouldRunWritesSequentially()
    {
        var probe = new DispatchConcurrencyProbe();
        await using var scope = CreateParallelScope(
            probe,
            builder => builder.AddMethodHandler<ExclusiveWriteHandler>());

        var dispatcher = scope.ServiceProvider.GetRequiredService<IJmapRequestDispatcher>();

        await dispatcher.DispatchAsync(
            CreateTransaction(scope.ServiceProvider),
            CoreRequest(
                JmapRequestBuilder.ExclusiveWrite("c1", 100, "account1"),
                JmapRequestBuilder.ExclusiveWrite("c2", 100, "account1")));

        await Assert.That(probe.MaxObservedConcurrency).IsEqualTo(1);
    }

    private static AsyncServiceScope CreateParallelScope(
        DispatchConcurrencyProbe probe,
        Action<JMAP.Net.Hosting.Builders.JmapServerBuilder> configureServer)
    {
        var services = new ServiceCollection();

        services.AddSingleton(probe);
        services
            .AddJmap()
            .AddServer(builder =>
            {
                builder.AddSessionProvider<CoreSessionProvider>();
                builder.EnableParallelDispatch(maxDegreeOfParallelism: 4);
                configureServer(builder);
            })
            .UseAspNetCore();

        return services.BuildServiceProvider(validateScopes: true).CreateAsyncScope();
    }
}
