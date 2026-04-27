using System.Net;
using System.Net.Http.Json;
using System.Text;

using JMAP.Net.Common.Errors;
using JMAP.Net.Common.Protocol;
using JMAP.Net.Common.Session;
using JMAP.Net.Tests.Hosting.Infrastructure;

namespace JMAP.Net.Tests.Hosting;

public class JmapEndpointHttpTests
{
    [Test]
    public async Task SessionEndpoint_WhenRequested_ShouldReturnSessionDocument()
    {
        await using var factory = new JmapWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/.well-known/jmap");
        var session = await response.Content.ReadFromJsonAsync<JmapSession>();

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        await Assert.That(session).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(session!.Username).IsEqualTo("user@example.com");
        await Assert.That(session.State).IsEqualTo("session-state");
    }

    [Test]
    public async Task ApiEndpoint_WhenRequestIsValid_ShouldReturnMethodResponse()
    {
        await using var factory = new JmapWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/jmap",
            JmapRequestBuilder.CoreRequest(JmapRequestBuilder.CoreEcho("c1")));
        var envelope = await response.Content.ReadFromJsonAsync<JmapResponse>();

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        await Assert.That(envelope).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(envelope!.SessionState).IsEqualTo("session-state");
        await Assert.That(envelope.MethodResponses.Count).IsEqualTo(1);
        await Assert.That(envelope.MethodResponses[0].Name).IsEqualTo("Core/echo");
        await Assert.That(envelope.MethodResponses[0].MethodCallId).IsEqualTo("c1");
    }

    [Test]
    public async Task ApiEndpoint_WhenBodyIsInvalidJson_ShouldReturnNotJsonProblem()
    {
        await using var factory = new JmapWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsync(
            "/jmap",
            new StringContent("{ invalid", Encoding.UTF8, "application/json"));
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        await Assert.That(problem).IsNotNull();
        await Assert.That(problem!.Type).IsEqualTo(ProblemDetailsType.NotJson);
    }

    [Test]
    public async Task ApiEndpoint_WhenBodyIsNotJmapRequest_ShouldReturnNotRequestProblem()
    {
        await using var factory = new JmapWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsync(
            "/jmap",
            new StringContent("""{"using":["urn:ietf:params:jmap:core"]}""", Encoding.UTF8, "application/json"));
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        await Assert.That(problem).IsNotNull();
        await Assert.That(problem!.Type).IsEqualTo(ProblemDetailsType.NotRequest);
    }

    [Test]
    public async Task ApiEndpoint_WhenMaxCallsInRequestIsExceeded_ShouldReturnLimitProblem()
    {
        await using var factory = new JmapWebApplicationFactory(
            configureServer: server => server.SetMaxCallsInRequest(1));
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/jmap",
            JmapRequestBuilder.CoreRequest(
                JmapRequestBuilder.CoreEcho("c1"),
                JmapRequestBuilder.CoreEcho("c2")));
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        await Assert.That(problem).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(problem!.Type).IsEqualTo(ProblemDetailsType.Limit);
        await Assert.That(problem.Limit).IsEqualTo("maxCallsInRequest");
    }

    [Test]
    public async Task ApiEndpoint_WhenCapabilityIsUnsupported_ShouldReturnUnknownCapabilityProblem()
    {
        await using var factory = new JmapWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/jmap",
            JmapRequestBuilder.UnsupportedCapabilityRequest(JmapRequestBuilder.CoreEcho("c1")));
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        await Assert.That(problem).IsNotNull();
        await Assert.That(problem!.Type).IsEqualTo(ProblemDetailsType.UnknownCapability);
    }

    [Test]
    public async Task SessionEndpoint_WhenHttpsIsRequiredAndRequestIsHttp_ShouldReturnNotRequestProblem()
    {
        await using var factory = new JmapWebApplicationFactory(
            configureAspNetCore: aspNetCore => aspNetCore.RequireHttps());
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/.well-known/jmap");
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        await Assert.That(problem).IsNotNull();
        await Assert.That(problem!.Type).IsEqualTo(ProblemDetailsType.NotRequest);
    }
}
