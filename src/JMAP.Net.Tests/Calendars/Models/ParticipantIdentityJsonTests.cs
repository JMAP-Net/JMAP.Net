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

public class ParticipantIdentityJsonTests
{
    [Test]
    public async Task ParticipantIdentityWithDefaultName_WhenSerialized_ShouldMatchFixture()
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
    public async Task ParticipantIdentity_WhenDeserializedFromFixture_ShouldApplyDefaultName()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "participant-identity.json"));

        var identity = JsonSerializer.Deserialize<ParticipantIdentity>(json);

        await Assert.That(identity).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(identity!.Id).IsEqualTo(new JmapId("ident1"));
        await Assert.That(identity.Name).IsEqualTo(string.Empty);
        await Assert.That(identity.SendTo["imip"]).IsEqualTo("mailto:user@example.com");
    }
}
