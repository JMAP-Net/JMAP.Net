using JMAP.Net.TestServer.Handlers;
using JMAP.Net.TestServer.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddJmap()
    .AddServer(server =>
    {
        server.AddSessionProvider<TestServerSessionProvider>();
        server.AddMethodHandler<CoreEchoHandler>();
        server.AddMethodHandler<PrincipalGetHandler>();
        server.AddMethodHandler<CalendarGetHandler>();
        server.AddMethodHandler<CalendarQueryHandler>();
        server.AddMethodHandler<CalendarChangesHandler>();
        server.AddMethodHandler<CalendarEventGetHandler>();
        server.AddMethodHandler<CalendarEventQueryHandler>();
        server.AddMethodHandler<CalendarEventChangesHandler>();
    })
    .UseAspNetCore();

builder.Services.AddSingleton<TestServerData>();

var app = builder.Build();

app.MapJmap();
app.MapGet("/", () => Results.Redirect("/.well-known/jmap"));

app.Run();
