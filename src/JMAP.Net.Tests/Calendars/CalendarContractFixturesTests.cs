using System.Text.Json;
using JMAP.Net.Capabilities.Calendars.Methods.Calendar;
using JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;
using JMAP.Net.Capabilities.Calendars.Methods.CalendarEventNotification;
using JMAP.Net.Capabilities.Calendars.Methods.ParticipantIdentity;
using JMAP.Net.Capabilities.Core.Methods.Query;
using JMAP.Net.Capabilities.Calendars.Types;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Common.Errors;
using JMAP.Net.Tests.Infrastructure;
using JSCalendar.Net;
using JSCalendar.Net.Enums;
using Task = System.Threading.Tasks.Task;

namespace JMAP.Net.Tests.Calendars;

public class CalendarContractFixturesTests
{
    [Test]
    public async Task Serialize_Calendar_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CreateCalendar());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar.json"));
    }

    [Test]
    public async Task Deserialize_CalendarFixture_ShouldReadShareWithAndRights()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar.json"));

        var calendar = JsonSerializer.Deserialize<Calendar>(json);

        await Assert.That(calendar).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(calendar!.Id).IsEqualTo(new JmapId("cal1"));
        await Assert.That(calendar.ShareWith!.ContainsKey(new JmapId("principal1"))).IsTrue();
        await Assert.That(calendar.MyRights.MayWriteAll).IsTrue();
    }

    [Test]
    public async Task Serialize_CalendarEvent_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CreateCalendarEvent());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event.json"));
    }

    [Test]
    public async Task Deserialize_CalendarEventFixture_ShouldReadJmapSpecificFields()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event.json"));

        var calendarEvent = JsonSerializer.Deserialize<CalendarEvent>(json);

        await Assert.That(calendarEvent).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(calendarEvent!.Id).IsEqualTo(new JmapId("event1"));
        await Assert.That(calendarEvent.CalendarIds!.ContainsKey(new JmapId("cal1"))).IsTrue();
        await Assert.That(calendarEvent.IsDraft).IsTrue();
        await Assert.That(calendarEvent.MayInviteSelf).IsTrue();
        await Assert.That(calendarEvent.UtcStart!.Value.ToString()).IsEqualTo("2026-04-01T08:00:00Z");
        await Assert.That(calendarEvent.Participants!["alice"].Name).IsEqualTo("Alice");
    }

    [Test]
    public async Task Serialize_CalendarEventNotification_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CreateCalendarEventNotification());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-notification.json"));
    }

    [Test]
    public async Task Deserialize_CalendarEventNotificationFixture_ShouldReadNotificationShape()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-notification.json"));

        var notification = JsonSerializer.Deserialize<CalendarEventNotification>(json);

        await Assert.That(notification).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(notification!.Id).IsEqualTo(new JmapId("notif1"));
        await Assert.That(notification.Type).IsEqualTo(CalendarEventNotificationType.Updated);
        await Assert.That(notification.CalendarEventId).IsEqualTo(new JmapId("event1"));
        await Assert.That(notification.ChangedBy.PrincipalId).IsEqualTo(new JmapId("principal1"));
        await Assert.That(notification.EventPatch).IsNotNull();
    }

    [Test]
    public async Task Serialize_ParticipantIdentityWithDefaultName_ShouldMatchFixture()
    {
        var identity = new ParticipantIdentity
        {
            Id = new JmapId("ident1"),
            ScheduleId = "mailto:user@example.com",
            SendTo = new Dictionary<string, string>
            {
                ["imip"] = "mailto:user@example.com"
            },
            IsDefault = false
        };

        var json = JsonSerializer.Serialize(identity);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "participant-identity.json"));
    }

    [Test]
    public async Task Deserialize_ParticipantIdentityFixture_ShouldApplyDefaultName()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "participant-identity.json"));

        var identity = JsonSerializer.Deserialize<ParticipantIdentity>(json);

        await Assert.That(identity).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(identity!.Id).IsEqualTo(new JmapId("ident1"));
        await Assert.That(identity.Name).IsEqualTo(string.Empty);
        await Assert.That(identity.SendTo["imip"]).IsEqualTo("mailto:user@example.com");
    }

    [Test]
    public async Task Deserialize_CalendarGetResponseFixture_ShouldReadConcreteCalendarType()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-get-response.json"));

        var response = JsonSerializer.Deserialize<CalendarGetResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.AccountId).IsEqualTo(new JmapId("account1"));
        await Assert.That(response.List.Count).IsEqualTo(1);
        await Assert.That(response.List[0].Id).IsEqualTo(new JmapId("cal1"));
        await Assert.That(response.NotFound[0]).IsEqualTo(new JmapId("missing-cal"));
    }

    [Test]
    public async Task Deserialize_CalendarEventGetResponseFixture_ShouldReadConcreteEventType()
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
    public async Task Serialize_CalendarSetResponse_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CreateCalendarSetResponse());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-set-response.json"));
    }

    [Test]
    public async Task Deserialize_CalendarSetResponseFixture_ShouldReadTypedBuckets()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-set-response.json"));

        var response = JsonSerializer.Deserialize<CalendarSetResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.Created!.ContainsKey(new JmapId("temp-cal-1"))).IsTrue();
        await Assert.That(response.Created[new JmapId("temp-cal-1")].Id).IsEqualTo(new JmapId("cal1"));
        await Assert.That(response.Updated![new JmapId("cal2")]).IsNull();
        await Assert.That(response.Destroyed![0]).IsEqualTo(new JmapId("cal3"));
        await Assert.That(response.NotCreated![new JmapId("temp-cal-2")].Type).IsEqualTo(SetErrorType.InvalidProperties);
        await Assert.That(response.NotUpdated![new JmapId("cal4")].Type).IsEqualTo(SetErrorType.NotFound);
        await Assert.That(response.NotDestroyed![new JmapId("cal5")].Type).IsEqualTo(SetErrorType.Forbidden);
    }

    [Test]
    public async Task Serialize_CalendarEventSetResponse_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CreateCalendarEventSetResponse());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-set-response.json"));
    }

    [Test]
    public async Task Deserialize_CalendarEventSetResponseFixture_ShouldReadTypedBuckets()
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
    public async Task Deserialize_ParticipantIdentityGetResponseFixture_ShouldReadConcreteIdentityType()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "participant-identity-get-response.json"));

        var response = JsonSerializer.Deserialize<ParticipantIdentityGetResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.AccountId).IsEqualTo(new JmapId("account1"));
        await Assert.That(response.List.Count).IsEqualTo(1);
        await Assert.That(response.List[0].Id).IsEqualTo(new JmapId("ident1"));
        await Assert.That(response.List[0].Name).IsEqualTo(string.Empty);
        await Assert.That(response.NotFound[0]).IsEqualTo(new JmapId("missing-identity"));
    }

    [Test]
    public async Task Serialize_ParticipantIdentitySetResponse_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CreateParticipantIdentitySetResponse());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "participant-identity-set-response.json"));
    }

    [Test]
    public async Task Deserialize_ParticipantIdentitySetResponseFixture_ShouldReadTypedBuckets()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "participant-identity-set-response.json"));

        var response = JsonSerializer.Deserialize<ParticipantIdentitySetResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.Created!.ContainsKey(new JmapId("temp-ident-1"))).IsTrue();
        await Assert.That(response.Created[new JmapId("temp-ident-1")].ScheduleId).IsEqualTo("mailto:user@example.com");
        await Assert.That(response.Updated![new JmapId("ident2")]).IsNull();
        await Assert.That(response.Destroyed![0]).IsEqualTo(new JmapId("ident3"));
        await Assert.That(response.NotCreated![new JmapId("temp-ident-2")].Type).IsEqualTo(SetErrorType.InvalidProperties);
        await Assert.That(response.NotUpdated![new JmapId("ident4")].Type).IsEqualTo(SetErrorType.NotFound);
        await Assert.That(response.NotDestroyed![new JmapId("ident5")].Type).IsEqualTo(SetErrorType.Forbidden);
    }

    [Test]
    public async Task Deserialize_CalendarEventNotificationGetResponseFixture_ShouldReadConcreteNotificationType()
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
    public async Task Serialize_CalendarEventNotificationSetResponse_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CreateCalendarEventNotificationSetResponse());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-notification-set-response.json"));
    }

    [Test]
    public async Task Deserialize_CalendarEventNotificationSetResponseFixture_ShouldReadTypedBuckets()
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
    public async Task Deserialize_CalendarChangesResponseFixture_ShouldReadBuckets()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-changes-response.json"));

        var response = JsonSerializer.Deserialize<CalendarChangesResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.OldState).IsEqualTo("state-1");
        await Assert.That(response.NewState).IsEqualTo("state-2");
        await Assert.That(response.HasMoreChanges).IsFalse();
        await Assert.That(response.Created[0]).IsEqualTo(new JmapId("cal1"));
        await Assert.That(response.Updated[0]).IsEqualTo(new JmapId("cal2"));
        await Assert.That(response.Destroyed[0]).IsEqualTo(new JmapId("cal3"));
    }

    [Test]
    public async Task Serialize_CalendarQueryResponse_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CreateCalendarQueryResponse());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-query-response.json"));
    }

    [Test]
    public async Task Deserialize_CalendarQueryResponseFixture_ShouldReadIdsAndTotals()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-query-response.json"));

        var response = JsonSerializer.Deserialize<CalendarQueryResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.AccountId).IsEqualTo(new JmapId("account1"));
        await Assert.That(response.Ids.Count).IsEqualTo(2);
        await Assert.That(response.Ids[0]).IsEqualTo(new JmapId("cal1"));
        await Assert.That(response.Total!.Value.Value).IsEqualTo(2L);
        await Assert.That(response.Limit!.Value.Value).IsEqualTo(25L);
    }

    [Test]
    public async Task Deserialize_CalendarQueryChangesResponseFixture_ShouldReadAddedAndRemoved()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-query-changes-response.json"));

        var response = JsonSerializer.Deserialize<CalendarQueryChangesResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.OldQueryState).IsEqualTo("query-state-1");
        await Assert.That(response.NewQueryState).IsEqualTo("query-state-2");
        await Assert.That(response.Removed[0]).IsEqualTo(new JmapId("cal1"));
        await Assert.That(response.Added[0].Id).IsEqualTo(new JmapId("cal3"));
        await Assert.That(response.Added[0].Index.Value).IsEqualTo(0L);
    }

    [Test]
    public async Task Serialize_CalendarCopyResponse_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CreateCalendarCopyResponse());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-copy-response.json"));
    }

    [Test]
    public async Task Deserialize_CalendarCopyResponseFixture_ShouldReadTypedCreatedAndErrors()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-copy-response.json"));

        var response = JsonSerializer.Deserialize<CalendarCopyResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.FromAccountId).IsEqualTo(new JmapId("source-account"));
        await Assert.That(response.AccountId).IsEqualTo(new JmapId("target-account"));
        await Assert.That(response.Created![new JmapId("copy-cal-1")].Id).IsEqualTo(new JmapId("cal1"));
        await Assert.That(response.NotCreated![new JmapId("copy-cal-2")].Type).IsEqualTo(SetErrorType.AlreadyExists);
    }

    [Test]
    public async Task Deserialize_CalendarEventChangesResponseFixture_ShouldReadBuckets()
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
    public async Task Serialize_CalendarEventQueryResponse_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CreateCalendarEventQueryResponse());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-query-response.json"));
    }

    [Test]
    public async Task Deserialize_CalendarEventQueryResponseFixture_ShouldReadIdsAndTotals()
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
    public async Task Deserialize_CalendarEventQueryChangesResponseFixture_ShouldReadAddedAndRemoved()
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
    public async Task Serialize_CalendarEventCopyResponse_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CreateCalendarEventCopyResponse());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-copy-response.json"));
    }

    [Test]
    public async Task Deserialize_CalendarEventCopyResponseFixture_ShouldReadTypedCreatedAndErrors()
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

    [Test]
    public async Task Deserialize_ParticipantIdentityChangesResponseFixture_ShouldReadBuckets()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "participant-identity-changes-response.json"));

        var response = JsonSerializer.Deserialize<ParticipantIdentityChangesResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.Created[0]).IsEqualTo(new JmapId("ident1"));
        await Assert.That(response.Updated[0]).IsEqualTo(new JmapId("ident2"));
        await Assert.That(response.Destroyed[0]).IsEqualTo(new JmapId("ident3"));
    }

    [Test]
    public async Task Deserialize_CalendarEventNotificationChangesResponseFixture_ShouldReadBuckets()
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
    public async Task Serialize_CalendarEventNotificationQueryResponse_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CreateCalendarEventNotificationQueryResponse());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-notification-query-response.json"));
    }

    [Test]
    public async Task Deserialize_CalendarEventNotificationQueryResponseFixture_ShouldReadIdsAndTotals()
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
    public async Task Deserialize_CalendarEventNotificationQueryChangesResponseFixture_ShouldReadAddedAndRemoved()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-event-notification-query-changes-response.json"));

        var response = JsonSerializer.Deserialize<CalendarEventNotificationQueryChangesResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.Removed[0]).IsEqualTo(new JmapId("notif1"));
        await Assert.That(response.Added[0].Id).IsEqualTo(new JmapId("notif3"));
        await Assert.That(response.Added[0].Index.Value).IsEqualTo(0L);
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

    private static CalendarEventNotification CreateCalendarEventNotification()
    {
        return new CalendarEventNotification
        {
            Id = new JmapId("notif1"),
            Created = new JmapUtcDate(new DateTimeOffset(2026, 4, 1, 9, 30, 0, TimeSpan.Zero)),
            ChangedBy = new NotificationPerson
            {
                Name = "Alice",
                Email = "alice@example.com",
                PrincipalId = new JmapId("principal1"),
                ScheduleId = "mailto:alice@example.com"
            },
            Comment = "Updated the agenda",
            Type = CalendarEventNotificationType.Updated,
            CalendarEventId = new JmapId("event1"),
            IsDraft = false,
            Event = CreateCalendarEvent(),
            EventPatch = new PatchObject(new Dictionary<string, object?>
            {
                ["title"] = "Design Review Updated"
            })
        };
    }

    private static CalendarSetResponse CreateCalendarSetResponse()
    {
        return new CalendarSetResponse
        {
            AccountId = new JmapId("account1"),
            OldState = "state-1",
            NewState = "state-2",
            Created = new Dictionary<JmapId, Calendar>
            {
                [new JmapId("temp-cal-1")] = CreateCalendar()
            },
            Updated = new Dictionary<JmapId, Calendar?>
            {
                [new JmapId("cal2")] = null
            },
            Destroyed =
            [
                new JmapId("cal3")
            ],
            NotCreated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("temp-cal-2")] = new SetError
                {
                    Type = SetErrorType.InvalidProperties,
                    Description = "name is required",
                    Properties =
                    [
                        "name"
                    ]
                }
            },
            NotUpdated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("cal4")] = new SetError
                {
                    Type = SetErrorType.NotFound,
                    Description = "Calendar not found"
                }
            },
            NotDestroyed = new Dictionary<JmapId, SetError>
            {
                [new JmapId("cal5")] = new SetError
                {
                    Type = SetErrorType.Forbidden,
                    Description = "Calendar cannot be destroyed"
                }
            }
        };
    }

    private static CalendarQueryResponse CreateCalendarQueryResponse()
    {
        return new CalendarQueryResponse
        {
            AccountId = new JmapId("account1"),
            QueryState = "query-state-1",
            CanCalculateChanges = true,
            Position = new JmapUnsignedInt(0),
            Ids =
            [
                new JmapId("cal1"),
                new JmapId("cal2")
            ],
            Total = new JmapUnsignedInt(2),
            Limit = new JmapUnsignedInt(25)
        };
    }

    private static CalendarCopyResponse CreateCalendarCopyResponse()
    {
        return new CalendarCopyResponse
        {
            FromAccountId = new JmapId("source-account"),
            AccountId = new JmapId("target-account"),
            OldState = "state-1",
            NewState = "state-2",
            Created = new Dictionary<JmapId, Calendar>
            {
                [new JmapId("copy-cal-1")] = CreateCalendar()
            },
            NotCreated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("copy-cal-2")] = new SetError
                {
                    Type = SetErrorType.AlreadyExists,
                    Description = "A calendar with this id already exists",
                    ExistingId = "cal-existing"
                }
            }
        };
    }

    private static CalendarEventSetResponse CreateCalendarEventSetResponse()
    {
        return new CalendarEventSetResponse
        {
            AccountId = new JmapId("account1"),
            OldState = "state-1",
            NewState = "state-2",
            Created = new Dictionary<JmapId, CalendarEvent>
            {
                [new JmapId("temp-event-1")] = CreateCalendarEvent()
            },
            Updated = new Dictionary<JmapId, CalendarEvent?>
            {
                [new JmapId("event2")] = null
            },
            Destroyed =
            [
                new JmapId("event3")
            ],
            NotCreated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("temp-event-2")] = new SetError
                {
                    Type = SetErrorType.InvalidProperties,
                    Description = "calendarIds is required",
                    Properties =
                    [
                        "calendarIds"
                    ]
                }
            },
            NotUpdated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("event4")] = new SetError
                {
                    Type = SetErrorType.NotFound,
                    Description = "Calendar event not found"
                }
            },
            NotDestroyed = new Dictionary<JmapId, SetError>
            {
                [new JmapId("event5")] = new SetError
                {
                    Type = SetErrorType.Forbidden,
                    Description = "Calendar event cannot be destroyed"
                }
            }
        };
    }

    private static CalendarEventQueryResponse CreateCalendarEventQueryResponse()
    {
        return new CalendarEventQueryResponse
        {
            AccountId = new JmapId("account1"),
            QueryState = "query-state-1",
            CanCalculateChanges = true,
            Position = new JmapUnsignedInt(0),
            Ids =
            [
                new JmapId("event1"),
                new JmapId("event2")
            ],
            Total = new JmapUnsignedInt(2),
            Limit = new JmapUnsignedInt(100)
        };
    }

    private static CalendarEventCopyResponse CreateCalendarEventCopyResponse()
    {
        return new CalendarEventCopyResponse
        {
            FromAccountId = new JmapId("source-account"),
            AccountId = new JmapId("target-account"),
            OldState = "state-1",
            NewState = "state-2",
            Created = new Dictionary<JmapId, CalendarEvent>
            {
                [new JmapId("copy-event-1")] = CreateCalendarEvent()
            },
            NotCreated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("copy-event-2")] = new SetError
                {
                    Type = SetErrorType.AlreadyExists,
                    Description = "A calendar event with this id already exists",
                    ExistingId = "event-existing"
                }
            }
        };
    }

    private static ParticipantIdentitySetResponse CreateParticipantIdentitySetResponse()
    {
        return new ParticipantIdentitySetResponse
        {
            AccountId = new JmapId("account1"),
            OldState = "state-1",
            NewState = "state-2",
            Created = new Dictionary<JmapId, ParticipantIdentity>
            {
                [new JmapId("temp-ident-1")] = CreateParticipantIdentity()
            },
            Updated = new Dictionary<JmapId, ParticipantIdentity?>
            {
                [new JmapId("ident2")] = null
            },
            Destroyed =
            [
                new JmapId("ident3")
            ],
            NotCreated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("temp-ident-2")] = new SetError
                {
                    Type = SetErrorType.InvalidProperties,
                    Description = "scheduleId is required",
                    Properties =
                    [
                        "scheduleId"
                    ]
                }
            },
            NotUpdated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("ident4")] = new SetError
                {
                    Type = SetErrorType.NotFound,
                    Description = "Participant identity not found"
                }
            },
            NotDestroyed = new Dictionary<JmapId, SetError>
            {
                [new JmapId("ident5")] = new SetError
                {
                    Type = SetErrorType.Forbidden,
                    Description = "Participant identity cannot be destroyed"
                }
            }
        };
    }

    private static CalendarEventNotificationSetResponse CreateCalendarEventNotificationSetResponse()
    {
        return new CalendarEventNotificationSetResponse
        {
            AccountId = new JmapId("account1"),
            OldState = "state-1",
            NewState = "state-2",
            Created = new Dictionary<JmapId, CalendarEventNotification>
            {
                [new JmapId("temp-notif-1")] = CreateCalendarEventNotification()
            },
            Updated = new Dictionary<JmapId, CalendarEventNotification?>
            {
                [new JmapId("notif2")] = null
            },
            Destroyed =
            [
                new JmapId("notif3")
            ],
            NotCreated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("temp-notif-2")] = new SetError
                {
                    Type = SetErrorType.InvalidProperties,
                    Description = "type is required",
                    Properties =
                    [
                        "type"
                    ]
                }
            },
            NotUpdated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("notif4")] = new SetError
                {
                    Type = SetErrorType.NotFound,
                    Description = "Calendar event notification not found"
                }
            },
            NotDestroyed = new Dictionary<JmapId, SetError>
            {
                [new JmapId("notif5")] = new SetError
                {
                    Type = SetErrorType.Forbidden,
                    Description = "Calendar event notification cannot be destroyed"
                }
            }
        };
    }

    private static CalendarEventNotificationQueryResponse CreateCalendarEventNotificationQueryResponse()
    {
        return new CalendarEventNotificationQueryResponse
        {
            AccountId = new JmapId("account1"),
            QueryState = "query-state-1",
            CanCalculateChanges = true,
            Position = new JmapUnsignedInt(0),
            Ids =
            [
                new JmapId("notif1"),
                new JmapId("notif2")
            ],
            Total = new JmapUnsignedInt(2),
            Limit = new JmapUnsignedInt(50)
        };
    }
}
