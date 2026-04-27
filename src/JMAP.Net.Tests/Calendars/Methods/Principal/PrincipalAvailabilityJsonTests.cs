using System.Text.Json;
using JMAP.Net.Capabilities.Calendars.Methods.Principal;
using JMAP.Net.Capabilities.Calendars.Types;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Tests.Calendars.Infrastructure;
using JMAP.Net.Tests.Common.Fixtures;

namespace JMAP.Net.Tests.Calendars.Methods.Principal;

public class PrincipalAvailabilityJsonTests
{
    [Test]
    public async Task PrincipalGetAvailabilityRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new PrincipalGetAvailabilityRequest
        {
            AccountId = new JmapId("account1"),
            Id = new JmapId("principal1"),
            UtcStart = new JmapUtcDate(new DateTimeOffset(2026, 4, 1, 0, 0, 0, TimeSpan.Zero)),
            UtcEnd = new JmapUtcDate(new DateTimeOffset(2026, 4, 2, 0, 0, 0, TimeSpan.Zero)),
            ShowDetails = true,
            EventProperties =
            [
                "id",
                "title",
                "utcStart",
                "utcEnd"
            ]
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "principal-get-availability-request.json"));
    }

    [Test]
    public async Task PrincipalGetAvailabilityRequest_WhenDeserializedFromFixture_ShouldReadArguments()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "principal-get-availability-request.json"));

        var request = JsonSerializer.Deserialize<PrincipalGetAvailabilityRequest>(json);

        await Assert.That(request).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(request!.AccountId).IsEqualTo(new JmapId("account1"));
        await Assert.That(request.Id).IsEqualTo(new JmapId("principal1"));
        await Assert.That(request.UtcStart).IsEqualTo(
            new JmapUtcDate(new DateTimeOffset(2026, 4, 1, 0, 0, 0, TimeSpan.Zero)));
        await Assert.That(request.UtcEnd).IsEqualTo(
            new JmapUtcDate(new DateTimeOffset(2026, 4, 2, 0, 0, 0, TimeSpan.Zero)));
        await Assert.That(request.ShowDetails).IsTrue();
        await Assert.That(request.EventProperties![0]).IsEqualTo("id");
    }

    [Test]
    public async Task PrincipalGetAvailabilityResponse_WhenSerialized_ShouldMatchFixture()
    {
        var response = new PrincipalGetAvailabilityResponse
        {
            List =
            [
                new BusyPeriod
                {
                    UtcStart = new JmapUtcDate(new DateTimeOffset(2026, 4, 1, 8, 0, 0, TimeSpan.Zero)),
                    UtcEnd = new JmapUtcDate(new DateTimeOffset(2026, 4, 1, 9, 0, 0, TimeSpan.Zero)),
                    BusyStatus = BusyStatus.Confirmed,
                    AccountId = new JmapId("account1"),
                    Event = CalendarFixtures.CreateCalendarEvent()
                },
                new BusyPeriod
                {
                    UtcStart = new JmapUtcDate(new DateTimeOffset(2026, 4, 1, 12, 0, 0, TimeSpan.Zero)),
                    UtcEnd = new JmapUtcDate(new DateTimeOffset(2026, 4, 1, 13, 0, 0, TimeSpan.Zero))
                }
            ]
        };

        var json = JsonSerializer.Serialize(response);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "principal-get-availability-response.json"));
    }

    [Test]
    public async Task PrincipalGetAvailabilityResponse_WhenDeserializedFromFixture_ShouldReadBusyPeriods()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "principal-get-availability-response.json"));

        var response = JsonSerializer.Deserialize<PrincipalGetAvailabilityResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.List.Count).IsEqualTo(2);
        await Assert.That(response.List[0].BusyStatus).IsEqualTo(BusyStatus.Confirmed);
        await Assert.That(response.List[0].AccountId).IsEqualTo(new JmapId("account1"));
        await Assert.That(response.List[0].Event!.Id).IsEqualTo(new JmapId("event1"));
        await Assert.That(response.List[1].BusyStatus).IsEqualTo(BusyStatus.Unavailable);
        await Assert.That(response.List[1].Event).IsNull();
    }

    [Test]
    public async Task BusyStatus_WhenSerialized_ShouldUseRfcStringValues()
    {
        await Assert.That(JsonSerializer.Serialize(BusyStatus.Confirmed)).IsEqualTo("\"confirmed\"");
        await Assert.That(JsonSerializer.Serialize(BusyStatus.Tentative)).IsEqualTo("\"tentative\"");
        await Assert.That(JsonSerializer.Serialize(BusyStatus.Unavailable)).IsEqualTo("\"unavailable\"");
    }
}
