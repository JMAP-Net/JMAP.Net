using System.Text.Json;
using JMAP.Net.Capabilities.Core.Methods;
using JMAP.Net.Capabilities.Core.Methods.Query;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Common.Errors;
using JMAP.Net.Tests.Infrastructure;

namespace JMAP.Net.Tests.Protocol;

public class JmapQueryAndErrorsTests
{
    [Test]
    public async Task Serialize_QueryResponse_ShouldMatchFixture()
    {
        var response = new QueryResponse
        {
            AccountId = new JmapId("account1"),
            QueryState = "query-state-1",
            CanCalculateChanges = true,
            Position = new JmapUnsignedInt(0),
            Ids = [new JmapId("item1"), new JmapId("item2"), new JmapId("item3")],
            Total = new JmapUnsignedInt(3),
            Limit = new JmapUnsignedInt(50)
        };

        var json = JsonSerializer.Serialize(response);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Protocol", "query-response-with-total.json"));
    }

    [Test]
    public async Task Deserialize_QueryResponseFixture_ShouldReadIdsAndTotals()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Protocol", "query-response-with-total.json"));

        var response = JsonSerializer.Deserialize<QueryResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.AccountId).IsEqualTo(new JmapId("account1"));
        await Assert.That(response.QueryState).IsEqualTo("query-state-1");
        await Assert.That(response.CanCalculateChanges).IsTrue();
        await Assert.That(response.Ids.Count).IsEqualTo(3);
        await Assert.That(response.Total!.Value.Value).IsEqualTo(3L);
        await Assert.That(response.Limit!.Value.Value).IsEqualTo(50L);
    }

    [Test]
    public async Task Deserialize_QueryChangesResponseFixture_ShouldReadAddedAndRemoved()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Protocol", "query-changes-response.json"));

        var response = JsonSerializer.Deserialize<QueryChangesResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.OldQueryState).IsEqualTo("query-state-1");
        await Assert.That(response.NewQueryState).IsEqualTo("query-state-2");
        await Assert.That(response.Removed.Count).IsEqualTo(1);
        await Assert.That(response.Removed[0]).IsEqualTo(new JmapId("item1"));
        await Assert.That(response.Added.Count).IsEqualTo(2);
        await Assert.That(response.Added[0].Id).IsEqualTo(new JmapId("item4"));
        await Assert.That(response.Added[0].Index.Value).IsEqualTo(0L);
        await Assert.That(response.Total!.Value.Value).IsEqualTo(4L);
    }

    [Test]
    public async Task Deserialize_ChangesResponseFixture_ShouldReadChangeBuckets()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Protocol", "changes-response.json"));

        var response = JsonSerializer.Deserialize<ChangesResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.OldState).IsEqualTo("state-1");
        await Assert.That(response.NewState).IsEqualTo("state-2");
        await Assert.That(response.HasMoreChanges).IsFalse();
        await Assert.That(response.Created.Count).IsEqualTo(1);
        await Assert.That(response.Updated.Count).IsEqualTo(2);
        await Assert.That(response.Destroyed.Count).IsEqualTo(1);
        await Assert.That(response.Destroyed[0]).IsEqualTo(new JmapId("item1"));
    }

    [Test]
    public async Task Serialize_ProblemDetailsWithExtensionData_ShouldMatchFixture()
    {
        var details = new ProblemDetails
        {
            Type = ProblemDetailsType.Limit,
            Status = 429,
            Detail = "The request exceeded the maximum number of method calls.",
            Limit = "maxCallsInRequest",
            ExtensionData = new Dictionary<string, object?>
            {
                ["maxCallsInRequest"] = 16
            }
        };

        var json = JsonSerializer.Serialize(details);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Protocol", "problem-details-limit.json"));
    }

    [Test]
    public async Task Deserialize_ProblemDetailsFixture_ShouldReadExtensionData()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Protocol", "problem-details-limit.json"));

        var details = JsonSerializer.Deserialize<ProblemDetails>(json);

        await Assert.That(details).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(details!.Type).IsEqualTo(ProblemDetailsType.Limit);
        await Assert.That(details.Status).IsEqualTo(429);
        await Assert.That(details.Limit).IsEqualTo("maxCallsInRequest");
        await Assert.That(details.ExtensionData).IsNotNull();
        await Assert.That(details.ExtensionData!["maxCallsInRequest"]).IsTypeOf<JsonElement>();
        await Assert.That(((JsonElement)details.ExtensionData["maxCallsInRequest"]!).GetInt32()).IsEqualTo(16);
    }

    [Test]
    public async Task Serialize_JmapErrorWithExtensionData_ShouldMatchFixture()
    {
        var error = new JmapError
        {
            Type = JmapErrorType.InvalidArguments,
            Description = "The 'ids' argument is invalid.",
            ExtensionData = new Dictionary<string, object?>
            {
                ["arguments"] = new[] { "ids" }
            }
        };

        var json = JsonSerializer.Serialize(error);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Protocol", "jmap-error-invalid-arguments.json"));
    }

    [Test]
    public async Task Deserialize_JmapErrorFixture_ShouldReadExtensionData()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Protocol", "jmap-error-invalid-arguments.json"));

        var error = JsonSerializer.Deserialize<JmapError>(json);

        await Assert.That(error).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(error!.Type).IsEqualTo(JmapErrorType.InvalidArguments);
        await Assert.That(error.Description).IsEqualTo("The 'ids' argument is invalid.");
        await Assert.That(error.ExtensionData).IsNotNull();
        await Assert.That(error.ExtensionData!["arguments"]).IsTypeOf<JsonElement>();
        await Assert.That(((JsonElement)error.ExtensionData["arguments"]!).GetArrayLength()).IsEqualTo(1);
    }
}
