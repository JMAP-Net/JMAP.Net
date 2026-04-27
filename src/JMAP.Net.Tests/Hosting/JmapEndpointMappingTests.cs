using JMAP.Net.Tests.Hosting.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace JMAP.Net.Tests.Hosting;

public class JmapEndpointMappingTests
{
    [Test]
    public async Task MapJmap_WhenConfigured_ShouldExposeSessionAndApiRoutes()
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services
            .AddJmap()
            .AddServer(server =>
            {
                server.SetApiPath("/api/jmap");
                server.SetSessionPath("/.well-known/jmap");
                server.AddSessionProvider<CoreSessionProvider>();
            })
            .UseAspNetCore();

        var app = builder.Build();

        app.MapJmap();

        var routeEndpoints = ((IEndpointRouteBuilder)app).DataSources
            .SelectMany(dataSource => dataSource.Endpoints)
            .OfType<RouteEndpoint>()
            .Select(endpoint => endpoint.RoutePattern.RawText)
            .ToList();

        using var _ = Assert.Multiple();
        await Assert.That(routeEndpoints).Contains("/api/jmap");
        await Assert.That(routeEndpoints).Contains("/.well-known/jmap");
    }
}
