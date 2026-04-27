using System.Text.Json;
using JMAP.Net.Capabilities.Calendars.Methods.Calendar;
using JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;
using JMAP.Net.Capabilities.Calendars.Methods.CalendarEventNotification;
using JMAP.Net.Capabilities.Calendars.Methods.ParticipantIdentity;
using JMAP.Net.Capabilities.Calendars.Types;
using JMAP.Net.Capabilities.Core.Methods.Query;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Tests.Calendars.Infrastructure;
using JMAP.Net.Tests.Common.Fixtures;
using JSCalendar.Net;
using JSCalendar.Net.Enums;
using Task = System.Threading.Tasks.Task;

namespace JMAP.Net.Tests.Calendars.Methods.CalendarEventNotificationMethods;

public class CalendarEventNotificationRequestJsonTests
{
    [Test]
    public async Task CalendarEventNotificationGetRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new CalendarEventNotificationGetRequest
        {
            AccountId = new JmapId("account1"),
            Ids = [new JmapId("notif1"), new JmapId("notif2")],
            Properties = ["type", "calendarEventId", "created"]
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-notification-get-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarEventNotificationGetRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-notification-get-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.Ids!.Count).IsEqualTo(2);
        await Assert.That(deserialized.Properties![0]).IsEqualTo("type");
    }

    [Test]
    public async Task CalendarEventNotificationSetRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new CalendarEventNotificationSetRequest
        {
            AccountId = new JmapId("account1"),
            IfInState = "state-1",
            Destroy = [new JmapId("notif1"), new JmapId("notif2")]
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-notification-set-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarEventNotificationSetRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-notification-set-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.IfInState).IsEqualTo("state-1");
        await Assert.That(deserialized.Destroy!.Count).IsEqualTo(2);
    }

    [Test]
    public async Task CalendarEventNotificationQueryRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new CalendarEventNotificationQueryRequest
        {
            AccountId = new JmapId("account1"),
            Filter = new CalendarEventNotificationFilterCondition
            {
                After = new JmapUtcDate(new DateTimeOffset(2026, 4, 1, 0, 0, 0, TimeSpan.Zero)),
                Before = new JmapUtcDate(new DateTimeOffset(2026, 5, 1, 0, 0, 0, TimeSpan.Zero)),
                Type = CalendarEventNotificationType.Updated,
                CalendarEventIds = [new JmapId("event1"), new JmapId("event2")]
            },
            Sort =
            [
                new Comparator
                {
                    Property = "created",
                    IsAscending = false
                }
            ],
            Position = new JmapInt(0),
            Limit = new JmapUnsignedInt(50),
            CalculateTotal = true
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-notification-query-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarEventNotificationQueryRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-notification-query-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.Limit!.Value.Value).IsEqualTo(50L);
        await Assert.That(deserialized.CalculateTotal).IsTrue();
        await Assert.That(deserialized.Filter).IsTypeOf<JsonElement>();
    }

    [Test]
    public async Task CalendarEventNotificationQueryChangesRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new CalendarEventNotificationQueryChangesRequest
        {
            AccountId = new JmapId("account1"),
            Filter = new CalendarEventNotificationFilterCondition
            {
                Type = CalendarEventNotificationType.Updated,
                CalendarEventIds = [new JmapId("event1")]
            },
            Sort =
            [
                new Comparator
                {
                    Property = "created",
                    IsAscending = false
                }
            ],
            SinceQueryState = "query-state-1",
            MaxChanges = new JmapUnsignedInt(10),
            UpToId = new JmapId("notif2"),
            CalculateTotal = true
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-notification-query-changes-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarEventNotificationQueryChangesRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-notification-query-changes-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.SinceQueryState).IsEqualTo("query-state-1");
        await Assert.That(deserialized.UpToId).IsEqualTo(new JmapId("notif2"));
        await Assert.That(deserialized.Filter).IsTypeOf<JsonElement>();
    }

    [Test]
    public async Task CalendarEventNotificationChangesRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new CalendarEventNotificationChangesRequest
        {
            AccountId = new JmapId("account1"),
            SinceState = "state-1",
            MaxChanges = new JmapUnsignedInt(30)
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-notification-changes-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarEventNotificationChangesRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-notification-changes-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.SinceState).IsEqualTo("state-1");
        await Assert.That(deserialized.MaxChanges!.Value.Value).IsEqualTo(30L);
    }
}
