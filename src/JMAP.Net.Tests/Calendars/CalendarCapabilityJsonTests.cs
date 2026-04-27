using System.Text.Json;
using JMAP.Net.Capabilities.Calendars;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Tests.Common.Fixtures;

namespace JMAP.Net.Tests.Calendars;

public class CalendarCapabilityJsonTests
{
    [Test]
    public async Task CalendarCapability_WhenSerialized_ShouldBeEmptyObject()
    {
        var capability = new CalendarCapability();

        var json = JsonSerializer.Serialize(capability);

        await Assert.That(json).IsEqualTo("{}");
    }

    [Test]
    public async Task CalendarAccountCapability_WhenSerialized_ShouldWriteAccountProperties()
    {
        var capability = new CalendarAccountCapability
        {
            MaxCalendarsPerEvent = new JmapUnsignedInt(3),
            MinDateTime = "1900-01-01T00:00:00Z",
            MaxDateTime = "2100-01-01T00:00:00Z",
            MaxExpandedQueryDuration = "P1Y",
            MaxParticipantsPerEvent = null,
            MayCreateCalendar = true
        };

        var json = JsonSerializer.Serialize(capability);

        using var actual = JsonDocument.Parse(json);
        using var expected = JsonDocument.Parse(
            """
            {
              "maxCalendarsPerEvent": 3,
              "minDateTime": "1900-01-01T00:00:00Z",
              "maxDateTime": "2100-01-01T00:00:00Z",
              "maxExpandedQueryDuration": "P1Y",
              "mayCreateCalendar": true
            }
            """);

        await Assert.That(JsonElement.DeepEquals(actual.RootElement, expected.RootElement)).IsTrue();
    }

    [Test]
    public async Task CalendarAccountCapability_CapabilityUri_ShouldMatchSessionCapabilityUri()
    {
        await Assert.That(CalendarAccountCapability.CapabilityUri).IsEqualTo(CalendarCapability.CapabilityUri);
    }

    [Test]
    public async Task CalendarParseCapability_WhenSerialized_ShouldBeEmptyObject()
    {
        var capability = new CalendarParseCapability();

        var json = JsonSerializer.Serialize(capability);

        await Assert.That(json).IsEqualTo("{}");
        await Assert.That(CalendarParseCapability.CapabilityUri).IsEqualTo("urn:ietf:params:jmap:calendars:parse");
    }

    [Test]
    public async Task PrincipalAvailabilityCapability_WhenSerialized_ShouldBeEmptyObject()
    {
        var capability = new PrincipalAvailabilityCapability();

        var json = JsonSerializer.Serialize(capability);

        await Assert.That(json).IsEqualTo("{}");
        await Assert.That(PrincipalAvailabilityCapability.CapabilityUri)
            .IsEqualTo("urn:ietf:params:jmap:principals:availability");
    }

    [Test]
    public async Task PrincipalAvailabilityAccountCapability_WhenSerialized_ShouldWriteMaxAvailabilityDuration()
    {
        var capability = new PrincipalAvailabilityAccountCapability
        {
            MaxAvailabilityDuration = "P90D"
        };

        var json = JsonSerializer.Serialize(capability);

        await Assert.That(json).IsEqualTo("""{"maxAvailabilityDuration":"P90D"}""");
        await Assert.That(PrincipalAvailabilityAccountCapability.CapabilityUri)
            .IsEqualTo(PrincipalAvailabilityCapability.CapabilityUri);
    }

    [Test]
    public async Task CalendarPrincipalCapability_WhenSerialized_ShouldMatchFixture()
    {
        var capability = new CalendarPrincipalCapability
        {
            AccountId = new JmapId("account1"),
            MayGetAvailability = true,
            MayShareWith = true,
            CalendarAddress = "mailto:principal@example.com"
        };

        var json = JsonSerializer.Serialize(capability);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-principal-capability.json"));
        await Assert.That(CalendarPrincipalCapability.CapabilityUri).IsEqualTo(CalendarCapability.CapabilityUri);
    }

    [Test]
    public async Task CalendarPrincipalCapability_WhenDeserializedFromFixture_ShouldReadProperties()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-principal-capability.json"));

        var capability = JsonSerializer.Deserialize<CalendarPrincipalCapability>(json);

        await Assert.That(capability).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(capability!.AccountId).IsEqualTo(new JmapId("account1"));
        await Assert.That(capability.MayGetAvailability).IsTrue();
        await Assert.That(capability.MayShareWith).IsTrue();
        await Assert.That(capability.CalendarAddress).IsEqualTo("mailto:principal@example.com");
    }
}
