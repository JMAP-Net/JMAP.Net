using JMAP.Net.Hosting.Configuration;
using JMAP.Net.Hosting.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Exposes extension methods used to map the JMAP endpoints into the ASP.NET Core routing pipeline.
/// </summary>
public static class MapJmapEndpointRouteBuilderExtensions
{
    /// <summary>
    /// Maps the configured JMAP session and API endpoints.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The route group containing the mapped endpoints.</returns>
    public static RouteGroupBuilder MapJmap(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var options = endpoints.ServiceProvider.GetRequiredService<IOptions<JmapServerOptions>>().Value;
        var group = endpoints.MapGroup(string.Empty);

        group.MapGet(options.SessionPath, JmapEndpointHandlers.HandleSessionAsync);
        group.MapPost(options.ApiPath, JmapEndpointHandlers.HandleApiAsync);

        return group;
    }
}
