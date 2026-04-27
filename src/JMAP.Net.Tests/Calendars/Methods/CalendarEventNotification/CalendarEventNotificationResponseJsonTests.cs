using System.Text.Json;
using JMAP.Net.Capabilities.Calendars.Methods.Calendar;
using JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;
using JMAP.Net.Capabilities.Calendars.Methods.CalendarEventNotification;
using JMAP.Net.Capabilities.Calendars.Methods.ParticipantIdentity;
using JMAP.Net.Capabilities.Calendars.Types;
using JMAP.Net.Capabilities.Core.Methods.Query;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Common.Errors;
using JMAP.Net.Tests.Calendars.Infrastructure;
using JMAP.Net.Tests.Common.Fixtures;
using JSCalendar.Net;
using JSCalendar.Net.Enums;
using Task = System.Threading.Tasks.Task;

namespace JMAP.Net.Tests.Calendars.Methods.CalendarEventNotificationMethods;

public class CalendarEventNotificationResponseJsonTests
{
    [Test]
    public async Task CalendarEventNotificationGetResponse_WhenDeserializedFromFixture_ShouldReadConcreteNotificationType()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-notification-get-response.json"));

        var response = JsonSerializer.Deserialize<CalendarEventNotificationGetResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.AccountId).IsEqualTo(new JmapId("account1"));
        await Assert.That(response.List.Count).IsEqualTo(1);
        await Assert.That(response.List[0].Id).IsEqualTo(new JmapId("notif1"));
        await Assert.That(response.List[0].Type).IsEqualTo(CalendarEventNotificationType.Updated);
        await Assert.That(response.NotFound[0]).IsEqualTo(new JmapId("missing-notification"));
    }

    [Test]
    public async Task CalendarEventNotificationSetResponse_WhenSerialized_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CalendarFixtures.CreateCalendarEventNotificationSetResponse());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-notification-set-response.json"));
    }

    [Test]
    public async Task CalendarEventNotificationSetResponse_WhenDeserializedFromFixture_ShouldReadTypedBuckets()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-notification-set-response.json"));

        var response = JsonSerializer.Deserialize<CalendarEventNotificationSetResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.Created!.ContainsKey(new JmapId("temp-notif-1"))).IsTrue();
        await Assert.That(response.Created[new JmapId("temp-notif-1")].CalendarEventId).IsEqualTo(new JmapId("event1"));
        await Assert.That(response.Updated![new JmapId("notif2")]).IsNull();
        await Assert.That(response.Destroyed![0]).IsEqualTo(new JmapId("notif3"));
        await Assert.That(response.NotCreated![new JmapId("temp-notif-2")].Type).IsEqualTo(SetErrorType.InvalidProperties);
        await Assert.That(response.NotUpdated![new JmapId("notif4")].Type).IsEqualTo(SetErrorType.NotFound);
        await Assert.That(response.NotDestroyed![new JmapId("notif5")].Type).IsEqualTo(SetErrorType.Forbidden);
    }

    [Test]
    public async Task CalendarEventNotificationChangesResponse_WhenDeserializedFromFixture_ShouldReadBuckets()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-notification-changes-response.json"));

        var response = JsonSerializer.Deserialize<CalendarEventNotificationChangesResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.Created[0]).IsEqualTo(new JmapId("notif1"));
        await Assert.That(response.Updated[0]).IsEqualTo(new JmapId("notif2"));
        await Assert.That(response.Destroyed[0]).IsEqualTo(new JmapId("notif3"));
    }

    [Test]
    public async Task CalendarEventNotificationQueryResponse_WhenSerialized_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CalendarFixtures.CreateCalendarEventNotificationQueryResponse());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-notification-query-response.json"));
    }

    [Test]
    public async Task CalendarEventNotificationQueryResponse_WhenDeserializedFromFixture_ShouldReadIdsAndTotals()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-notification-query-response.json"));

        var response = JsonSerializer.Deserialize<CalendarEventNotificationQueryResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.Ids.Count).IsEqualTo(2);
        await Assert.That(response.Ids[0]).IsEqualTo(new JmapId("notif1"));
        await Assert.That(response.Total!.Value.Value).IsEqualTo(2L);
        await Assert.That(response.Limit!.Value.Value).IsEqualTo(50L);
    }

    [Test]
    public async Task CalendarEventNotificationQueryChangesResponse_WhenDeserializedFromFixture_ShouldReadAddedAndRemoved()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-notification-query-changes-response.json"));

        var response = JsonSerializer.Deserialize<CalendarEventNotificationQueryChangesResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.Removed[0]).IsEqualTo(new JmapId("notif1"));
        await Assert.That(response.Added[0].Id).IsEqualTo(new JmapId("notif3"));
        await Assert.That(response.Added[0].Index.Value).IsEqualTo(0L);
    }
}
