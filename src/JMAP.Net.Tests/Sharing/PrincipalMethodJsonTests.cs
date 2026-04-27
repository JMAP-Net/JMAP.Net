using System.Text.Json;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Capabilities.Sharing.Methods.Principal;
using JMAP.Net.Capabilities.Sharing.Types;
using JMAP.Net.Tests.Common.Fixtures;

namespace JMAP.Net.Tests.Sharing;

public class PrincipalMethodJsonTests
{
    [Test]
    public async Task PrincipalGetRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new PrincipalGetRequest
        {
            AccountId = new JmapId("principal-account"),
            Ids =
            [
                new JmapId("principal1")
            ],
            Properties =
            [
                "id",
                "type",
                "name",
                "capabilities"
            ]
        };

        var json = JsonSerializer.Serialize(request);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Sharing", "principal-get-request.json"));
    }

    [Test]
    public async Task PrincipalGetResponse_WhenSerialized_ShouldMatchFixture()
    {
        var response = new PrincipalGetResponse
        {
            AccountId = new JmapId("principal-account"),
            State = "principal-state-1",
            List =
            [
                PrincipalJsonTests.CreatePrincipal()
            ],
            NotFound =
            [
                new JmapId("missing-principal")
            ]
        };

        var json = JsonSerializer.Serialize(response);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Sharing", "principal-get-response.json"));
    }

    [Test]
    public async Task PrincipalGetResponse_WhenDeserializedFromFixture_ShouldReadConcretePrincipalType()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Sharing", "principal-get-response.json"));

        var response = JsonSerializer.Deserialize<PrincipalGetResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.AccountId).IsEqualTo(new JmapId("principal-account"));
        await Assert.That(response.List[0].Type).IsEqualTo(PrincipalType.Individual);
        await Assert.That(response.NotFound[0]).IsEqualTo(new JmapId("missing-principal"));
    }

    [Test]
    public async Task PrincipalQueryRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new PrincipalQueryRequest
        {
            AccountId = new JmapId("principal-account"),
            Filter = new PrincipalFilterCondition
            {
                Name = "Jane",
                Type = PrincipalType.Individual,
                CalendarAddress = "mailto:jane@example.com"
            },
            CalculateTotal = true
        };

        var json = JsonSerializer.Serialize(request);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Sharing", "principal-query-request.json"));
    }

    [Test]
    public async Task PrincipalQueryResponse_WhenDeserializedFromFixture_ShouldReadIdsAndTotal()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Sharing", "principal-query-response.json"));

        var response = JsonSerializer.Deserialize<PrincipalQueryResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.AccountId).IsEqualTo(new JmapId("principal-account"));
        await Assert.That(response.Ids[0]).IsEqualTo(new JmapId("principal1"));
        await Assert.That(response.Total!.Value.Value).IsEqualTo(1L);
    }
}
