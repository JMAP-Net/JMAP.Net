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

namespace JMAP.Net.Tests.Calendars.Methods.CalendarEventMethods;

public class CalendarEventResponseJsonTests
{
    [Test]
    public async Task CalendarEventGetResponse_WhenDeserializedFromFixture_ShouldReadConcreteEventType()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-get-response.json"));

        var response = JsonSerializer.Deserialize<CalendarEventGetResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.AccountId).IsEqualTo(new JmapId("account1"));
        await Assert.That(response.List.Count).IsEqualTo(1);
        await Assert.That(response.List[0].Id).IsEqualTo(new JmapId("event1"));
        await Assert.That(response.List[0].CalendarIds!.ContainsKey(new JmapId("cal1"))).IsTrue();
        await Assert.That(response.NotFound[0]).IsEqualTo(new JmapId("missing-event"));
    }

    [Test]
    public async Task CalendarEventSetResponse_WhenSerialized_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CalendarFixtures.CreateCalendarEventSetResponse());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-set-response.json"));
    }

    [Test]
    public async Task CalendarEventSetResponse_WhenDeserializedFromFixture_ShouldReadTypedBuckets()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-set-response.json"));

        var response = JsonSerializer.Deserialize<CalendarEventSetResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.Created!.ContainsKey(new JmapId("temp-event-1"))).IsTrue();
        await Assert.That(response.Created[new JmapId("temp-event-1")].Id).IsEqualTo(new JmapId("event1"));
        await Assert.That(response.Updated![new JmapId("event2")]).IsNull();
        await Assert.That(response.Destroyed![0]).IsEqualTo(new JmapId("event3"));
        await Assert.That(response.NotCreated![new JmapId("temp-event-2")].Type).IsEqualTo(SetErrorType.InvalidProperties);
        await Assert.That(response.NotUpdated![new JmapId("event4")].Type).IsEqualTo(SetErrorType.NotFound);
        await Assert.That(response.NotDestroyed![new JmapId("event5")].Type).IsEqualTo(SetErrorType.Forbidden);
    }

    [Test]
    public async Task CalendarEventChangesResponse_WhenDeserializedFromFixture_ShouldReadBuckets()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-changes-response.json"));

        var response = JsonSerializer.Deserialize<CalendarEventChangesResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.OldState).IsEqualTo("state-1");
        await Assert.That(response.NewState).IsEqualTo("state-2");
        await Assert.That(response.Created[0]).IsEqualTo(new JmapId("event1"));
        await Assert.That(response.Updated[0]).IsEqualTo(new JmapId("event2"));
        await Assert.That(response.Destroyed[0]).IsEqualTo(new JmapId("event3"));
    }

    [Test]
    public async Task CalendarEventQueryResponse_WhenSerialized_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CalendarFixtures.CreateCalendarEventQueryResponse());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-query-response.json"));
    }

    [Test]
    public async Task CalendarEventQueryResponse_WhenDeserializedFromFixture_ShouldReadIdsAndTotals()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-query-response.json"));

        var response = JsonSerializer.Deserialize<CalendarEventQueryResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.Ids.Count).IsEqualTo(2);
        await Assert.That(response.Ids[0]).IsEqualTo(new JmapId("event1"));
        await Assert.That(response.Total!.Value.Value).IsEqualTo(2L);
        await Assert.That(response.Limit!.Value.Value).IsEqualTo(100L);
    }

    [Test]
    public async Task CalendarEventQueryChangesResponse_WhenDeserializedFromFixture_ShouldReadAddedAndRemoved()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-query-changes-response.json"));

        var response = JsonSerializer.Deserialize<CalendarEventQueryChangesResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.Removed[0]).IsEqualTo(new JmapId("event1"));
        await Assert.That(response.Added[0].Id).IsEqualTo(new JmapId("event3"));
        await Assert.That(response.Added[0].Index.Value).IsEqualTo(1L);
    }

    [Test]
    public async Task CalendarEventCopyResponse_WhenSerialized_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CalendarFixtures.CreateCalendarEventCopyResponse());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-copy-response.json"));
    }

    [Test]
    public async Task CalendarEventCopyResponse_WhenDeserializedFromFixture_ShouldReadTypedCreatedAndErrors()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-copy-response.json"));

        var response = JsonSerializer.Deserialize<CalendarEventCopyResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.FromAccountId).IsEqualTo(new JmapId("source-account"));
        await Assert.That(response.AccountId).IsEqualTo(new JmapId("target-account"));
        await Assert.That(response.Created![new JmapId("copy-event-1")].Id).IsEqualTo(new JmapId("event1"));
        await Assert.That(response.NotCreated![new JmapId("copy-event-2")].Type).IsEqualTo(SetErrorType.AlreadyExists);
    }
}
