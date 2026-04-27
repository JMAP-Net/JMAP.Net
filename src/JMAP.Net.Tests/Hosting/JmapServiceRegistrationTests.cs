using JMAP.Net.Hosting.Configuration;
using JMAP.Net.Hosting.Services;
using JMAP.Net.Tests.Hosting.Handlers;
using JMAP.Net.Tests.Hosting.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace JMAP.Net.Tests.Hosting;

public class JmapServiceRegistrationTests
{
    [Test]
    public async Task AddJmap_WhenConfigured_ShouldRegisterDispatcherAndOptions()
    {
        var services = new ServiceCollection();

        services
            .AddJmap()
            .AddServer(builder =>
            {
                builder.SetApiPath("/api/jmap");
                builder.SetSessionPath("/session/jmap");
                builder.AddSessionProvider<CoreSessionProvider>();
                builder.AddMethodHandler<CoreEchoHandler>();
            })
            .UseAspNetCore(builder =>
            {
                builder.RequireHttps();
                builder.EnableJsonResponseIndentation();
            });

        await using var scope = services.BuildServiceProvider(validateScopes: true).CreateAsyncScope();

        var dispatcher = scope.ServiceProvider.GetRequiredService<IJmapRequestDispatcher>();
        var serverOptions = scope.ServiceProvider.GetRequiredService<IOptions<JmapServerOptions>>().Value;
        var aspNetCoreOptions = scope.ServiceProvider.GetRequiredService<IOptions<JmapAspNetCoreOptions>>().Value;

        await Assert.That(dispatcher).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(serverOptions.ApiPath).IsEqualTo("/api/jmap");
        await Assert.That(serverOptions.SessionPath).IsEqualTo("/session/jmap");
        await Assert.That(aspNetCoreOptions.RequireHttps).IsTrue();
        await Assert.That(aspNetCoreOptions.WriteIndentedResponses).IsTrue();
    }
}
