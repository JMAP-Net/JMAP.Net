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

namespace JMAP.Net.Tests.Calendars.Methods.CalendarEventMethods;

public class CalendarEventRequestJsonTests
{
    [Test]
    public async Task CalendarEventGetRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new CalendarEventGetRequest
        {
            AccountId = new JmapId("account1"),
            Ids = [new JmapId("event1"), new JmapId("event2")],
            Properties = ["title", "start", "participants"],
            RecurrenceOverridesBefore = new JmapUtcDate(new DateTimeOffset(2026, 5, 1, 0, 0, 0, TimeSpan.Zero)),
            RecurrenceOverridesAfter = new JmapUtcDate(new DateTimeOffset(2026, 4, 1, 0, 0, 0, TimeSpan.Zero)),
            ReduceParticipants = true,
            TimeZone = "Europe/Berlin"
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-get-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarEventGetRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-get-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.ReduceParticipants).IsTrue();
        await Assert.That(deserialized.TimeZone).IsEqualTo("Europe/Berlin");
        await Assert.That(deserialized.RecurrenceOverridesAfter!.Value.ToString()).IsEqualTo("2026-04-01T00:00:00Z");
    }

    [Test]
    public async Task CalendarEventSetRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new CalendarEventSetRequest
        {
            AccountId = new JmapId("account1"),
            IfInState = "state-1",
            Create = new Dictionary<JmapId, CalendarEvent>
            {
                [new JmapId("temp-event-1")] = CalendarFixtures.CreateCalendarEvent()
            },
            Update = new Dictionary<JmapId, PatchObject>
            {
                [new JmapId("event2")] = new(new Dictionary<string, object?>
                {
                    ["title"] = "Updated Review",
                    ["hideAttendees"] = false
                })
            },
            Destroy = [new JmapId("event3")],
            SendSchedulingMessages = true
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-set-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarEventSetRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-set-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.Create!.ContainsKey(new JmapId("temp-event-1"))).IsTrue();
        await Assert.That(deserialized.Update!.ContainsKey(new JmapId("event2"))).IsTrue();
        await Assert.That(deserialized.Destroy![0]).IsEqualTo(new JmapId("event3"));
        await Assert.That(deserialized.SendSchedulingMessages).IsTrue();
    }

    [Test]
    public async Task CalendarEventQueryRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new CalendarEventQueryRequest
        {
            AccountId = new JmapId("account1"),
            Filter = new CalendarEventFilterCondition
            {
                InCalendars = [new JmapId("cal1")],
                After = "2026-04-01",
                Before = "2026-05-01",
                Text = "design",
                Owner = "Alice",
                ParticipationStatus = "accepted"
            },
            Sort =
            [
                new Comparator
                {
                    Property = "start"
                }
            ],
            Position = new JmapInt(0),
            Anchor = new JmapId("event2"),
            AnchorOffset = new JmapInt(-1),
            Limit = new JmapUnsignedInt(100),
            CalculateTotal = true,
            ExpandRecurrences = true,
            TimeZone = "Europe/Berlin"
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-query-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarEventQueryRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-query-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.ExpandRecurrences).IsTrue();
        await Assert.That(deserialized.TimeZone).IsEqualTo("Europe/Berlin");
        await Assert.That(deserialized.Anchor).IsEqualTo(new JmapId("event2"));
        await Assert.That(deserialized.Filter).IsTypeOf<JsonElement>();
    }

    [Test]
    public async Task CalendarEventQueryChangesRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new CalendarEventQueryChangesRequest
        {
            AccountId = new JmapId("account1"),
            Filter = new CalendarEventFilterCondition
            {
                InCalendars = [new JmapId("cal1")],
                After = "2026-04-01",
                Before = "2026-05-01"
            },
            Sort =
            [
                new Comparator
                {
                    Property = "start"
                }
            ],
            SinceQueryState = "query-state-1",
            MaxChanges = new JmapUnsignedInt(10),
            UpToId = new JmapId("event2"),
            CalculateTotal = true
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-query-changes-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarEventQueryChangesRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-query-changes-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.SinceQueryState).IsEqualTo("query-state-1");
        await Assert.That(deserialized.UpToId).IsEqualTo(new JmapId("event2"));
        await Assert.That(deserialized.Filter).IsTypeOf<JsonElement>();
    }

    [Test]
    public async Task CalendarEventChangesRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new CalendarEventChangesRequest
        {
            AccountId = new JmapId("account1"),
            SinceState = "state-1",
            MaxChanges = new JmapUnsignedInt(50)
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-changes-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarEventChangesRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-changes-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.SinceState).IsEqualTo("state-1");
        await Assert.That(deserialized.MaxChanges!.Value.Value).IsEqualTo(50L);
    }

    [Test]
    public async Task CalendarEventCopyRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new CalendarEventCopyRequest
        {
            FromAccountId = new JmapId("source-account"),
            IfFromInState = "source-state-1",
            AccountId = new JmapId("target-account"),
            IfInState = "target-state-1",
            Create = new Dictionary<JmapId, CalendarEvent>
            {
                [new JmapId("copy-event-1")] = CalendarFixtures.CreateCalendarEvent()
            },
            OnSuccessDestroyOriginal = true,
            DestroyFromIfInState = "source-state-2"
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-copy-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarEventCopyRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-copy-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.Create.ContainsKey(new JmapId("copy-event-1"))).IsTrue();
        await Assert.That(deserialized.OnSuccessDestroyOriginal).IsTrue();
        await Assert.That(deserialized.DestroyFromIfInState).IsEqualTo("source-state-2");
    }
}
