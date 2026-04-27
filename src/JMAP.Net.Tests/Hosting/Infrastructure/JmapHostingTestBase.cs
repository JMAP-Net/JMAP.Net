using JMAP.Net.Common.Protocol;
using JMAP.Net.Hosting;
using JMAP.Net.Hosting.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace JMAP.Net.Tests.Hosting.Infrastructure;

public abstract class JmapHostingTestBase
{
    protected static AsyncServiceScope CreateServiceScope(Action<JmapServerBuilder>? configureServer = null)
    {
        var services = new ServiceCollection();

        services
            .AddJmap()
            .AddServer(builder =>
            {
                builder.AddSessionProvider<CoreSessionProvider>();
                configureServer?.Invoke(builder);
            })
            .UseAspNetCore();

        return services.BuildServiceProvider(validateScopes: true).CreateAsyncScope();
    }

    protected static JmapTransaction CreateTransaction(IServiceProvider services)
    {
        return new JmapTransaction
        {
            Services = services
        };
    }

    protected static JmapRequest CoreRequest(params Invocation[] invocations)
        => JmapRequestBuilder.CoreRequest(invocations);
}
