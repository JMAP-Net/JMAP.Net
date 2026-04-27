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

namespace JMAP.Net.Tests.Calendars.Models;

public class CalendarEventNotificationJsonTests
{
    [Test]
    public async Task CalendarEventNotification_WhenSerialized_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CalendarFixtures.CreateCalendarEventNotification());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event-notification.json"));
    }

    [Test]
    public async Task CalendarEventNotification_WhenDeserializedFromFixture_ShouldReadNotificationShape()
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
    public async Task CalendarEventNotificationType_WhenSerialized_ShouldUseRfcStringValues()
    {
        await Assert.That(JsonSerializer.Serialize(CalendarEventNotificationType.Created)).IsEqualTo("\"created\"");
        await Assert.That(JsonSerializer.Serialize(CalendarEventNotificationType.Updated)).IsEqualTo("\"updated\"");
        await Assert.That(JsonSerializer.Serialize(CalendarEventNotificationType.Destroyed)).IsEqualTo("\"destroyed\"");
    }
}
