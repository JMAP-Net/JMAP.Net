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

public class CalendarJsonTests
{
    [Test]
    public async Task Calendar_WhenSerialized_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CalendarFixtures.CreateCalendar());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar.json"));
    }

    [Test]
    public async Task Calendar_WhenDeserializedFromFixture_ShouldReadShareWithAndRights()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar.json"));

        var calendar = JsonSerializer.Deserialize<Calendar>(json);

        await Assert.That(calendar).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(calendar!.Id).IsEqualTo(new JmapId("cal1"));
        await Assert.That(calendar.ShareWith!.ContainsKey(new JmapId("principal1"))).IsTrue();
        await Assert.That(calendar.MyRights.MayWriteAll).IsTrue();
    }
}
