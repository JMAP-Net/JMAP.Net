using System.Text.Json;
using JMAP.Net.Capabilities.Core.Methods;
using JMAP.Net.Capabilities.Core.Methods.Query;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Tests.Common.Fixtures;

namespace JMAP.Net.Tests.Core.Methods;

public class QueryResponseJsonTests
{
    [Test]
    public async Task QueryResponse_WhenSerialized_ShouldMatchFixture()
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
    public async Task QueryResponse_WhenDeserializedFromFixture_ShouldReadIdsAndTotals()
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
    public async Task QueryChangesResponse_WhenDeserializedFromFixture_ShouldReadAddedAndRemoved()
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
    public async Task ChangesResponse_WhenDeserializedFromFixture_ShouldReadChangeBuckets()
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
}
