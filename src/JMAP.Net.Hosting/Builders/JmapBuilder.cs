using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace JMAP.Net.Hosting.Builders;

/// <summary>
/// Provides a shared entry point for configuring JMAP hosting services.
/// </summary>
public sealed class JmapBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JmapBuilder" /> class.
    /// </summary>
    /// <param name="services">The services collection.</param>
    public JmapBuilder(IServiceCollection services)
        => Services = services ?? throw new ArgumentNullException(nameof(services));

    /// <summary>
    /// Gets the services collection.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public IServiceCollection Services { get; }

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
