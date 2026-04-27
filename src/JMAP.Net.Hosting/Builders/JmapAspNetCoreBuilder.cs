using System.ComponentModel;
using JMAP.Net.Hosting.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JMAP.Net.Hosting.Builders;

/// <summary>
/// Exposes the methods used to configure the ASP.NET Core JMAP integration.
/// </summary>
public sealed class JmapAspNetCoreBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JmapAspNetCoreBuilder" /> class.
    /// </summary>
    /// <param name="services">The services collection.</param>
    public JmapAspNetCoreBuilder(IServiceCollection services)
        => Services = services ?? throw new ArgumentNullException(nameof(services));

    /// <summary>
    /// Gets the services collection.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public IServiceCollection Services { get; }

    /// <summary>
    /// Applies additional ASP.NET Core hosting options configuration.
    /// </summary>
    /// <param name="configuration">The configuration delegate.</param>
    /// <returns>The current builder instance.</returns>
    public JmapAspNetCoreBuilder Configure(Action<JmapAspNetCoreOptions> configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        Services.Configure(configuration);

        return this;
    }

    /// <summary>
    /// Requires HTTPS for the mapped JMAP endpoints.
    /// </summary>
    /// <returns>The current builder instance.</returns>
    public JmapAspNetCoreBuilder RequireHttps()
        => Configure(options => options.RequireHttps = true);

    /// <summary>
    /// Enables indentation for JSON responses returned by the ASP.NET Core host.
    /// </summary>
    /// <returns>The current builder instance.</returns>
    public JmapAspNetCoreBuilder EnableJsonResponseIndentation()
        => Configure(options => options.WriteIndentedResponses = true);

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object? obj) => base.Equals(obj);

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string? ToString() => base.ToString();
}
