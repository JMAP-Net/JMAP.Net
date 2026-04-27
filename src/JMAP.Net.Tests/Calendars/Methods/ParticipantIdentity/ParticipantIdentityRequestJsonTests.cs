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

namespace JMAP.Net.Tests.Calendars.Methods.ParticipantIdentityMethods;

public class ParticipantIdentityRequestJsonTests
{
    [Test]
    public async Task ParticipantIdentityGetRequest_WhenSerialized_ShouldMatchFixture()
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
    public async Task ParticipantIdentitySetRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new ParticipantIdentitySetRequest
        {
            AccountId = new JmapId("account1"),
            IfInState = "state-1",
            Create = new Dictionary<JmapId, ParticipantIdentity>
            {
                [new JmapId("temp-ident-1")] = CalendarFixtures.CreateParticipantIdentity()
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
    public async Task ParticipantIdentityChangesRequest_WhenSerialized_ShouldMatchFixture()
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
}
