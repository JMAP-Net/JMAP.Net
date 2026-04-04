using System.Text.Json;
using JMAP.Net.Capabilities.Calendars.Methods.Calendar;
using JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;
using JMAP.Net.Capabilities.Calendars.Methods.CalendarEventNotification;
using JMAP.Net.Capabilities.Calendars.Methods.ParticipantIdentity;
using JMAP.Net.Capabilities.Calendars.Types;
using JMAP.Net.Capabilities.Core.Methods.Query;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Tests.Infrastructure;
using JSCalendar.Net;
using JSCalendar.Net.Enums;
using Task = System.Threading.Tasks.Task;

namespace JMAP.Net.Tests.Calendars;

public class CalendarRequestFixturesTests
{
    [Test]
    public async Task CalendarGetRequest_ShouldMatchFixture()
    {
        var request = new CalendarGetRequest
        {
            AccountId = new JmapId("account1"),
            Ids = [new JmapId("cal1"), new JmapId("cal2")],
            Properties = ["name", "color", "myRights"]
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-get-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarGetRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-get-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.AccountId).IsEqualTo(new JmapId("account1"));
        await Assert.That(deserialized.Ids!.Count).IsEqualTo(2);
        await Assert.That(deserialized.Properties![0]).IsEqualTo("name");
    }

    [Test]
    public async Task CalendarSetRequest_ShouldMatchFixture()
    {
        var request = new CalendarSetRequest
        {
            AccountId = new JmapId("account1"),
            IfInState = "state-1",
            Create = new Dictionary<JmapId, Calendar>
            {
                [new JmapId("temp-cal-1")] = CreateCalendar()
            },
            Update = new Dictionary<JmapId, PatchObject>
            {
                [new JmapId("cal2")] = new(new Dictionary<string, object?>
                {
                    ["name"] = "Renamed Calendar",
                    ["color"] = "#FF5500"
                })
            },
            Destroy = [new JmapId("cal3")],
            OnDestroyRemoveEvents = true,
            OnSuccessSetIsDefault = new JmapId("cal1")
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-set-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarSetRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-set-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.Create!.ContainsKey(new JmapId("temp-cal-1"))).IsTrue();
        await Assert.That(deserialized.Update!.ContainsKey(new JmapId("cal2"))).IsTrue();
        await Assert.That(deserialized.Destroy![0]).IsEqualTo(new JmapId("cal3"));
        await Assert.That(deserialized.OnDestroyRemoveEvents).IsTrue();
        await Assert.That(deserialized.OnSuccessSetIsDefault).IsEqualTo(new JmapId("cal1"));
    }

    [Test]
    public async Task CalendarQueryRequest_ShouldMatchFixture()
    {
        var request = new CalendarQueryRequest
        {
            AccountId = new JmapId("account1"),
            Filter = new CalendarFilterCondition
            {
                Name = "Team",
                UsedForDataTypes = ["event"],
                IsSubscribed = true,
                SharedWith = new JmapId("principal1")
            },
            Sort =
            [
                new Comparator
                {
                    Property = "name",
                    IsAscending = false,
                    Collation = "i;unicode-casemap"
                }
            ],
            Position = new JmapInt(0),
            Limit = new JmapUnsignedInt(25),
            CalculateTotal = true
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-query-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarQueryRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-query-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.Limit!.Value.Value).IsEqualTo(25L);
        await Assert.That(deserialized.CalculateTotal).IsTrue();
        await Assert.That(deserialized.Filter).IsTypeOf<JsonElement>();
    }

    [Test]
    public async Task CalendarQueryChangesRequest_ShouldMatchFixture()
    {
        var request = new CalendarQueryChangesRequest
        {
            AccountId = new JmapId("account1"),
            Filter = new CalendarFilterCondition
            {
                Name = "Team",
                IsSubscribed = true
            },
            Sort =
            [
                new Comparator
                {
                    Property = "name"
                }
            ],
            SinceQueryState = "query-state-1",
            MaxChanges = new JmapUnsignedInt(10),
            UpToId = new JmapId("cal2"),
            CalculateTotal = true
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-query-changes-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarQueryChangesRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-query-changes-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.SinceQueryState).IsEqualTo("query-state-1");
        await Assert.That(deserialized.MaxChanges!.Value.Value).IsEqualTo(10L);
        await Assert.That(deserialized.UpToId).IsEqualTo(new JmapId("cal2"));
        await Assert.That(deserialized.Filter).IsTypeOf<JsonElement>();
    }

    [Test]
    public async Task CalendarChangesRequest_ShouldMatchFixture()
    {
        var request = new CalendarChangesRequest
        {
            AccountId = new JmapId("account1"),
            SinceState = "state-1",
            MaxChanges = new JmapUnsignedInt(20)
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-changes-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarChangesRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-changes-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.SinceState).IsEqualTo("state-1");
        await Assert.That(deserialized.MaxChanges!.Value.Value).IsEqualTo(20L);
    }

    [Test]
    public async Task CalendarCopyRequest_ShouldMatchFixture()
    {
        var request = new CalendarCopyRequest
        {
            FromAccountId = new JmapId("source-account"),
            IfFromInState = "source-state-1",
            AccountId = new JmapId("target-account"),
            IfInState = "target-state-1",
            Create = new Dictionary<JmapId, Calendar>
            {
                [new JmapId("copy-cal-1")] = CreateCalendar()
            },
            OnSuccessDestroyOriginal = true,
            DestroyFromIfInState = "source-state-2"
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-copy-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarCopyRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-copy-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.FromAccountId).IsEqualTo(new JmapId("source-account"));
        await Assert.That(deserialized.Create.ContainsKey(new JmapId("copy-cal-1"))).IsTrue();
        await Assert.That(deserialized.OnSuccessDestroyOriginal).IsTrue();
        await Assert.That(deserialized.DestroyFromIfInState).IsEqualTo("source-state-2");
    }

    [Test]
    public async Task CalendarEventGetRequest_ShouldMatchFixture()
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
    public async Task CalendarEventSetRequest_ShouldMatchFixture()
    {
        var request = new CalendarEventSetRequest
        {
            AccountId = new JmapId("account1"),
            IfInState = "state-1",
            Create = new Dictionary<JmapId, CalendarEvent>
            {
                [new JmapId("temp-event-1")] = CreateCalendarEvent()
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
    public async Task CalendarEventQueryRequest_ShouldMatchFixture()
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
    public async Task CalendarEventQueryChangesRequest_ShouldMatchFixture()
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
    public async Task CalendarEventChangesRequest_ShouldMatchFixture()
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
    public async Task CalendarEventCopyRequest_ShouldMatchFixture()
    {
        var request = new CalendarEventCopyRequest
        {
            FromAccountId = new JmapId("source-account"),
            IfFromInState = "source-state-1",
            AccountId = new JmapId("target-account"),
            IfInState = "target-state-1",
            Create = new Dictionary<JmapId, CalendarEvent>
            {
                [new JmapId("copy-event-1")] = CreateCalendarEvent()
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

    [Test]
    public async Task ParticipantIdentityGetRequest_ShouldMatchFixture()
    {
        var request = new ParticipantIdentityGetRequest
        {
            AccountId = new JmapId("account1"),
            Ids = [new JmapId("ident1"), new JmapId("ident2")],
            Properties = ["name", "scheduleId", "isDefault"]
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "participant-identity-get-request.json"));

        var deserialized = JsonSerializer.Deserialize<ParticipantIdentityGetRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "participant-identity-get-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.Ids!.Count).IsEqualTo(2);
        await Assert.That(deserialized.Properties![1]).IsEqualTo("scheduleId");
    }

    [Test]
    public async Task ParticipantIdentitySetRequest_ShouldMatchFixture()
    {
        var request = new ParticipantIdentitySetRequest
        {
            AccountId = new JmapId("account1"),
            IfInState = "state-1",
            Create = new Dictionary<JmapId, ParticipantIdentity>
            {
                [new JmapId("temp-ident-1")] = CreateParticipantIdentity()
            },
            Update = new Dictionary<JmapId, PatchObject>
            {
                [new JmapId("ident2")] = new(new Dictionary<string, object?>
                {
                    ["name"] = "Updated Name"
                })
            },
            Destroy = [new JmapId("ident3")],
            OnSuccessSetIsDefault = new JmapId("ident1")
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "participant-identity-set-request.json"));

        var deserialized = JsonSerializer.Deserialize<ParticipantIdentitySetRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "participant-identity-set-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.Create!.ContainsKey(new JmapId("temp-ident-1"))).IsTrue();
        await Assert.That(deserialized.Destroy![0]).IsEqualTo(new JmapId("ident3"));
        await Assert.That(deserialized.OnSuccessSetIsDefault).IsEqualTo(new JmapId("ident1"));
    }

    [Test]
    public async Task ParticipantIdentityChangesRequest_ShouldMatchFixture()
    {
        var request = new ParticipantIdentityChangesRequest
        {
            AccountId = new JmapId("account1"),
            SinceState = "state-1",
            MaxChanges = new JmapUnsignedInt(15)
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "participant-identity-changes-request.json"));

        var deserialized = JsonSerializer.Deserialize<ParticipantIdentityChangesRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "participant-identity-changes-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.SinceState).IsEqualTo("state-1");
        await Assert.That(deserialized.MaxChanges!.Value.Value).IsEqualTo(15L);
    }

    [Test]
    public async Task CalendarEventNotificationGetRequest_ShouldMatchFixture()
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
    public async Task CalendarEventNotificationSetRequest_ShouldMatchFixture()
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
    public async Task CalendarEventNotificationQueryRequest_ShouldMatchFixture()
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
    public async Task CalendarEventNotificationQueryChangesRequest_ShouldMatchFixture()
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
    public async Task CalendarEventNotificationChangesRequest_ShouldMatchFixture()
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

    private static Calendar CreateCalendar()
    {
        return new Calendar
        {
            Id = new JmapId("cal1"),
            Name = "Team Calendar",
            Description = "Shared calendar",
            Color = "#00AAFF",
            SortOrder = new JmapUnsignedInt(5),
            IsSubscribed = true,
            IsVisible = true,
            IsDefault = false,
            IncludeInAvailability = "all",
            TimeZone = "Europe/Berlin",
            ShareWith = new Dictionary<JmapId, CalendarRights>
            {
                [new JmapId("principal1")] = new()
                {
                    MayReadFreeBusy = true,
                    MayReadItems = true,
                    MayWriteAll = false,
                    MayWriteOwn = true,
                    MayUpdatePrivate = true,
                    MayRSVP = true,
                    MayAdmin = false,
                    MayDelete = false
                }
            },
            MyRights = new CalendarRights
            {
                MayReadFreeBusy = true,
                MayReadItems = true,
                MayWriteAll = true,
                MayWriteOwn = true,
                MayUpdatePrivate = true,
                MayRSVP = true,
                MayAdmin = true,
                MayDelete = false
            }
        };
    }

    private static CalendarEvent CreateCalendarEvent()
    {
        return new CalendarEvent
        {
            Id = new JmapId("event1"),
            Uid = "uid-1@example.com",
            Updated = new DateTimeOffset(2026, 4, 1, 7, 30, 0, TimeSpan.Zero),
            Start = new LocalDateTime(new DateTime(2026, 4, 1, 10, 0, 0)),
            Duration = new Duration { Hours = 1 },
            Title = "Design Review",
            TimeZone = "Europe/Berlin",
            CalendarIds = new Dictionary<JmapId, bool>
            {
                [new JmapId("cal1")] = true
            },
            IsDraft = true,
            IsOrigin = true,
            UtcStart = new JmapUtcDate(new DateTimeOffset(2026, 4, 1, 8, 0, 0, TimeSpan.Zero)),
            UtcEnd = new JmapUtcDate(new DateTimeOffset(2026, 4, 1, 9, 0, 0, TimeSpan.Zero)),
            MayInviteSelf = true,
            MayInviteOthers = true,
            HideAttendees = true,
            Participants = new Dictionary<string, Participant>
            {
                ["alice"] = new JmapParticipant
                {
                    Name = "Alice",
                    Email = "alice@example.com",
                    ScheduleId = "mailto:alice@example.com",
                    Roles = new Dictionary<ParticipantRole, bool>
                    {
                        [ParticipantRole.Owner] = true
                    },
                    ParticipationStatus = ParticipationStatus.Accepted
                }
            }
        };
    }

    private static ParticipantIdentity CreateParticipantIdentity()
    {
        return new ParticipantIdentity
        {
            Id = new JmapId("ident1"),
            ScheduleId = "mailto:user@example.com",
            SendTo = new Dictionary<string, string>
            {
                ["imip"] = "mailto:user@example.com"
            },
            IsDefault = false
        };
    }
}
