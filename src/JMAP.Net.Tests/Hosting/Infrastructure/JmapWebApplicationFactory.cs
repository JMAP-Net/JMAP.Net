using JMAP.Net.Hosting.Builders;
using JMAP.Net.Tests.Hosting.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace JMAP.Net.Tests.Hosting.Infrastructure;

internal sealed class JmapWebApplicationFactory(
    Action<JmapServerBuilder>? configureServer = null,
    Action<JmapAspNetCoreBuilder>? configureAspNetCore = null,
    Action<IServiceCollection>? configureServices = null)
    : WebApplicationFactory<JmapEndpointHttpTests>
{
    protected override IWebHostBuilder? CreateWebHostBuilder()
    {
#pragma warning disable ASPDEPR004
        return new WebHostBuilder()
#pragma warning restore ASPDEPR004
            .ConfigureServices(services =>
            {
                configureServices?.Invoke(services);

                services
                    .AddJmap()
                    .AddServer(server =>
                    {
                        server.AddSessionProvider<CoreSessionProvider>();
                        server.AddMethodHandler<CoreEchoHandler>();
                        configureServer?.Invoke(server);
                    })
                    .UseAspNetCore(aspNetCore => configureAspNetCore?.Invoke(aspNetCore));
            })
            .Configure(app =>
            {
                app.UseRouting();
                app.UseEndpoints(endpoints => endpoints.MapJmap());
            });
    }
}
