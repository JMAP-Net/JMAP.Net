using System.Text.Json;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Capabilities.Sharing.Types;
using JMAP.Net.Tests.Common.Fixtures;

namespace JMAP.Net.Tests.Sharing;

public class ShareNotificationJsonTests
{
    [Test]
    public async Task ShareNotification_WhenSerialized_ShouldMatchFixture()
    {
        var notification = CreateShareNotification();

        var json = JsonSerializer.Serialize(notification);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Sharing", "share-notification.json"));
    }

    [Test]
    public async Task ShareNotification_WhenDeserializedFromFixture_ShouldReadFields()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Sharing", "share-notification.json"));

        var notification = JsonSerializer.Deserialize<ShareNotification>(json);

        await Assert.That(notification).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(notification!.Id).IsEqualTo(new JmapId("share-notification1"));
        await Assert.That(notification.Created.Value).IsEqualTo(new DateTimeOffset(2026, 4, 27, 8, 30, 0, TimeSpan.Zero));
        await Assert.That(notification.ChangedBy.PrincipalId).IsEqualTo(new JmapId("principal1"));
        await Assert.That(notification.ObjectType).IsEqualTo("Calendar");
        await Assert.That(notification.OldRights).IsNull();
        await Assert.That(notification.NewRights!["mayReadItems"]).IsTrue();
    }

    internal static ShareNotification CreateShareNotification()
    {
        return new ShareNotification
        {
            Id = new JmapId("share-notification1"),
            Created = new JmapUtcDate(new DateTimeOffset(2026, 4, 27, 8, 30, 0, TimeSpan.Zero)),
            ChangedBy = new Entity
            {
                Name = "Jane Doe",
                Email = "jane@example.com",
                PrincipalId = new JmapId("principal1")
            },
            ObjectType = "Calendar",
            ObjectAccountId = new JmapId("account1"),
            ObjectId = new JmapId("calendar1"),
            OldRights = null,
            NewRights = new Dictionary<string, bool>
            {
                ["mayReadItems"] = true,
                ["mayReadFreeBusy"] = true
            },
            Name = "Team Calendar"
        };
    }
}
