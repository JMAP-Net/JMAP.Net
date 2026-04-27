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

namespace JMAP.Net.Tests.Calendars.Methods.ParticipantIdentityMethods;

public class ParticipantIdentityResponseJsonTests
{
    [Test]
    public async Task ParticipantIdentityGetResponse_WhenDeserializedFromFixture_ShouldReadConcreteIdentityType()
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
    public async Task ParticipantIdentitySetResponse_WhenSerialized_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CalendarFixtures.CreateParticipantIdentitySetResponse());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "participant-identity-set-response.json"));
    }

    [Test]
    public async Task ParticipantIdentitySetResponse_WhenDeserializedFromFixture_ShouldReadTypedBuckets()
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
    public async Task ParticipantIdentityChangesResponse_WhenDeserializedFromFixture_ShouldReadBuckets()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "participant-identity-changes-response.json"));

        var response = JsonSerializer.Deserialize<ParticipantIdentityChangesResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.Created[0]).IsEqualTo(new JmapId("ident1"));
        await Assert.That(response.Updated[0]).IsEqualTo(new JmapId("ident2"));
        await Assert.That(response.Destroyed[0]).IsEqualTo(new JmapId("ident3"));
    }
}
