using JMAP.Net.Hosting.Builders;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Exposes extension methods used to register JMAP hosting services.
/// </summary>
public static class JmapExtensions
{
    /// <summary>
    /// Provides a common entry point for registering JMAP hosting services.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <returns>A JMAP builder.</returns>
    public static JmapBuilder AddJmap(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return new JmapBuilder(services);
    }

    /// <summary>
    /// Provides a common entry point for registering JMAP hosting services.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration delegate.</param>
    /// <returns>The services collection.</returns>
    public static IServiceCollection AddJmap(this IServiceCollection services, Action<JmapBuilder> configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        configuration(services.AddJmap());

        return services;
    }
}
