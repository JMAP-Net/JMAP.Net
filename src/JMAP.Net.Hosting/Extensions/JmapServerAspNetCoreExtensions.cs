using JMAP.Net.Hosting.Builders;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Exposes extension methods used to register the ASP.NET Core JMAP integration.
/// </summary>
public static class JmapServerAspNetCoreExtensions
{
    /// <summary>
    /// Registers the ASP.NET Core specific JMAP hosting services.
    /// </summary>
    /// <param name="builder">The JMAP server builder.</param>
    /// <returns>An ASP.NET Core builder.</returns>
    public static JmapAspNetCoreBuilder UseAspNetCore(this JmapServerBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddRouting();

        return new JmapAspNetCoreBuilder(builder.Services);
    }

    /// <summary>
    /// Registers the ASP.NET Core specific JMAP hosting services.
    /// </summary>
    /// <param name="builder">The JMAP server builder.</param>
    /// <param name="configuration">The configuration delegate.</param>
    /// <returns>The server builder.</returns>
    public static JmapServerBuilder UseAspNetCore(this JmapServerBuilder builder, Action<JmapAspNetCoreBuilder> configuration)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configuration);

        configuration(builder.UseAspNetCore());

        return builder;
    }
}
