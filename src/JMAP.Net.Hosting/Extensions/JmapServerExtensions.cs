using JMAP.Net.Hosting.Builders;
using JMAP.Net.Hosting.Configuration;
using JMAP.Net.Hosting.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Exposes extension methods used to register the JMAP server services.
/// </summary>
public static class JmapServerExtensions
{
    /// <summary>
    /// Registers the generic JMAP server services.
    /// </summary>
    /// <param name="builder">The root JMAP builder.</param>
    /// <returns>A server builder.</returns>
    public static JmapServerBuilder AddServer(this JmapBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddLogging();
        builder.Services.AddOptions();

        builder.Services.TryAddScoped<IJmapRequestDispatcher, JmapRequestDispatcher>();
        builder.Services.TryAddSingleton<IJmapRequestConcurrencyLimiter, JmapRequestConcurrencyLimiter>();
        builder.Services.TryAddSingleton<IJmapResultReferenceResolver, JmapResultReferenceResolver>();
        builder.Services.TryAddScoped<IJmapSessionProvider, MissingJmapSessionProvider>();

        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<
            IPostConfigureOptions<JmapServerOptions>, JmapServerConfiguration>());

        return new JmapServerBuilder(builder.Services);
    }

    /// <summary>
    /// Registers the generic JMAP server services.
    /// </summary>
    /// <param name="builder">The root JMAP builder.</param>
    /// <param name="configuration">The configuration delegate.</param>
    /// <returns>The server builder.</returns>
    public static JmapServerBuilder AddServer(this JmapBuilder builder, Action<JmapServerBuilder> configuration)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configuration);

        var serverBuilder = builder.AddServer();
        configuration(serverBuilder);

        return serverBuilder;
    }
}
