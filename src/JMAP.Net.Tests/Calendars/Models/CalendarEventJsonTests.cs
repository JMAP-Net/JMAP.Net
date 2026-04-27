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

public class CalendarEventJsonTests
{
    [Test]
    public async Task CalendarEvent_WhenSerialized_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CalendarFixtures.CreateCalendarEvent());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-event.json"));
    }

    [Test]
    public async Task CalendarEvent_WhenDeserializedFromFixture_ShouldReadJmapSpecificFields()
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
}
