using System.Text.Json;
using JMAP.Net.Capabilities.Core.Methods.Query;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Capabilities.Sharing.Methods.ShareNotification;
using JMAP.Net.Tests.Common.Fixtures;

namespace JMAP.Net.Tests.Sharing;

public class ShareNotificationMethodJsonTests
{
    [Test]
    public async Task ShareNotificationGetResponse_WhenSerialized_ShouldMatchFixture()
    {
        var response = new ShareNotificationGetResponse
        {
            AccountId = new JmapId("principal-account"),
            State = "share-notification-state-1",
            List =
            [
                ShareNotificationJsonTests.CreateShareNotification()
            ],
            NotFound =
            [
                new JmapId("missing-share-notification")
            ]
        };

        var json = JsonSerializer.Serialize(response);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Sharing", "share-notification-get-response.json"));
    }

    [Test]
    public async Task ShareNotificationGetResponse_WhenDeserializedFromFixture_ShouldReadConcreteNotificationType()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Sharing", "share-notification-get-response.json"));

        var response = JsonSerializer.Deserialize<ShareNotificationGetResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.AccountId).IsEqualTo(new JmapId("principal-account"));
        await Assert.That(response.List[0].ObjectId).IsEqualTo(new JmapId("calendar1"));
        await Assert.That(response.NotFound[0]).IsEqualTo(new JmapId("missing-share-notification"));
    }

    [Test]
    public async Task ShareNotificationQueryRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new ShareNotificationQueryRequest
        {
            AccountId = new JmapId("principal-account"),
            Filter = new ShareNotificationFilterCondition
            {
                After = new JmapUtcDate(new DateTimeOffset(2026, 4, 1, 0, 0, 0, TimeSpan.Zero)),
                Before = new JmapUtcDate(new DateTimeOffset(2026, 5, 1, 0, 0, 0, TimeSpan.Zero)),
                ObjectType = "Calendar",
                ObjectAccountId = new JmapId("account1")
            },
            Sort =
            [
                new Comparator
                {
                    Property = "created"
                }
            ],
            CalculateTotal = true
        };

        var json = JsonSerializer.Serialize(request);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Sharing", "share-notification-query-request.json"));
    }

    [Test]
    public async Task ShareNotificationQueryResponse_WhenDeserializedFromFixture_ShouldReadIdsAndTotal()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Sharing", "share-notification-query-response.json"));

        var response = JsonSerializer.Deserialize<ShareNotificationQueryResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.Ids[0]).IsEqualTo(new JmapId("share-notification1"));
        await Assert.That(response.Total!.Value.Value).IsEqualTo(1L);
    }

    [Test]
    public async Task ShareNotificationSetRequest_WhenSerializedForDestroy_ShouldMatchFixture()
    {
        var request = new ShareNotificationSetRequest
        {
            AccountId = new JmapId("principal-account"),
            Destroy =
            [
                new JmapId("share-notification1")
            ]
        };

        var json = JsonSerializer.Serialize(request);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Sharing", "share-notification-set-request.json"));
    }
}
