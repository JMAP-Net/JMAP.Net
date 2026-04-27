using System.Text.Json;
using JMAP.Net.Capabilities.Calendars;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Capabilities.Sharing.Types;
using JMAP.Net.Common.Session;
using JMAP.Net.Tests.Common.Fixtures;

namespace JMAP.Net.Tests.Sharing;

public class PrincipalJsonTests
{
    [Test]
    public async Task Principal_WhenSerialized_ShouldMatchFixture()
    {
        var principal = CreatePrincipal();

        var json = JsonSerializer.Serialize(principal);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Sharing", "principal.json"));
    }

    [Test]
    public async Task Principal_WhenDeserializedFromFixture_ShouldReadFieldsAndCalendarCapability()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Sharing", "principal.json"));

        var principal = JsonSerializer.Deserialize<Principal>(json);

        await Assert.That(principal).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(principal!.Id).IsEqualTo(new JmapId("principal1"));
        await Assert.That(principal.Type).IsEqualTo(PrincipalType.Individual);
        await Assert.That(principal.Capabilities.ContainsKey(CalendarCapability.CapabilityUri)).IsTrue();
        await Assert.That(principal.Accounts!.ContainsKey(new JmapId("account1"))).IsTrue();
    }

    [Test]
    public async Task PrincipalType_WhenSerialized_ShouldUseRfcStringValues()
    {
        await Assert.That(JsonSerializer.Serialize(PrincipalType.Individual)).IsEqualTo("\"individual\"");
        await Assert.That(JsonSerializer.Serialize(PrincipalType.Group)).IsEqualTo("\"group\"");
        await Assert.That(JsonSerializer.Serialize(PrincipalType.Resource)).IsEqualTo("\"resource\"");
        await Assert.That(JsonSerializer.Serialize(PrincipalType.Location)).IsEqualTo("\"location\"");
        await Assert.That(JsonSerializer.Serialize(PrincipalType.Other)).IsEqualTo("\"other\"");
    }

    internal static Principal CreatePrincipal()
    {
        return new Principal
        {
            Id = new JmapId("principal1"),
            Type = PrincipalType.Individual,
            Name = "Jane Doe",
            Description = "Calendar owner",
            Email = "jane@example.com",
            TimeZone = "Europe/Berlin",
            Capabilities = new Dictionary<string, object>
            {
                [CalendarCapability.CapabilityUri] = new CalendarPrincipalCapability
                {
                    AccountId = new JmapId("account1"),
                    MayGetAvailability = true,
                    MayShareWith = true,
                    CalendarAddress = "mailto:jane@example.com"
                }
            },
            Accounts = new Dictionary<JmapId, JmapAccount>
            {
                [new JmapId("account1")] = new()
                {
                    Name = "Jane Calendar",
                    IsPersonal = true,
                    IsReadOnly = false,
                    AccountCapabilities = new Dictionary<string, object>
                    {
                        [CalendarCapability.CapabilityUri] = new Dictionary<string, object>()
                    }
                }
            }
        };
    }
}
