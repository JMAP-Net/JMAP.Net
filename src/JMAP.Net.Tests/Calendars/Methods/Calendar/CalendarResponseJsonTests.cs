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

namespace JMAP.Net.Tests.Calendars.Methods.CalendarMethods;

public class CalendarResponseJsonTests
{
    [Test]
    public async Task CalendarGetResponse_WhenDeserializedFromFixture_ShouldReadConcreteCalendarType()
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
    public async Task CalendarSetResponse_WhenSerialized_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CalendarFixtures.CreateCalendarSetResponse());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-set-response.json"));
    }

    [Test]
    public async Task CalendarSetResponse_WhenDeserializedFromFixture_ShouldReadTypedBuckets()
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
    public async Task CalendarChangesResponse_WhenDeserializedFromFixture_ShouldReadBuckets()
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
    public async Task CalendarQueryResponse_WhenSerialized_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CalendarFixtures.CreateCalendarQueryResponse());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-query-response.json"));
    }

    [Test]
    public async Task CalendarQueryResponse_WhenDeserializedFromFixture_ShouldReadIdsAndTotals()
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
    public async Task CalendarQueryChangesResponse_WhenDeserializedFromFixture_ShouldReadAddedAndRemoved()
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
    public async Task CalendarCopyResponse_WhenSerialized_ShouldMatchFixture()
    {
        var json = JsonSerializer.Serialize(CalendarFixtures.CreateCalendarCopyResponse());

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-copy-response.json"));
    }

    [Test]
    public async Task CalendarCopyResponse_WhenDeserializedFromFixture_ShouldReadTypedCreatedAndErrors()
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
}
