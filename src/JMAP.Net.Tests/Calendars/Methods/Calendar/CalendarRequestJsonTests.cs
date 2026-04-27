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

namespace JMAP.Net.Tests.Calendars.Methods.CalendarMethods;

public class CalendarRequestJsonTests
{
    [Test]
    public async Task CalendarGetRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new CalendarGetRequest
        {
            AccountId = new JmapId("account1"),
            Ids = [new JmapId("cal1"), new JmapId("cal2")],
            Properties = ["name", "color", "myRights"]
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-get-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarGetRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-get-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.AccountId).IsEqualTo(new JmapId("account1"));
        await Assert.That(deserialized.Ids!.Count).IsEqualTo(2);
        await Assert.That(deserialized.Properties![0]).IsEqualTo("name");
    }

    [Test]
    public async Task CalendarSetRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new CalendarSetRequest
        {
            AccountId = new JmapId("account1"),
            IfInState = "state-1",
            Create = new Dictionary<JmapId, Calendar>
            {
                [new JmapId("temp-cal-1")] = CalendarFixtures.CreateCalendar()
            },
            Update = new Dictionary<JmapId, PatchObject>
            {
                [new JmapId("cal2")] = new(new Dictionary<string, object?>
                {
                    ["name"] = "Renamed Calendar",
                    ["color"] = "#FF5500"
                })
            },
            Destroy = [new JmapId("cal3")],
            OnDestroyRemoveEvents = true,
            OnSuccessSetIsDefault = new JmapId("cal1")
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-set-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarSetRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-set-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.Create!.ContainsKey(new JmapId("temp-cal-1"))).IsTrue();
        await Assert.That(deserialized.Update!.ContainsKey(new JmapId("cal2"))).IsTrue();
        await Assert.That(deserialized.Destroy![0]).IsEqualTo(new JmapId("cal3"));
        await Assert.That(deserialized.OnDestroyRemoveEvents).IsTrue();
        await Assert.That(deserialized.OnSuccessSetIsDefault).IsEqualTo(new JmapId("cal1"));
    }

    [Test]
    public async Task CalendarQueryRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new CalendarQueryRequest
        {
            AccountId = new JmapId("account1"),
            Filter = new CalendarFilterCondition
            {
                Name = "Team",
                UsedForDataTypes = ["event"],
                IsSubscribed = true,
                SharedWith = new JmapId("principal1")
            },
            Sort =
            [
                new Comparator
                {
                    Property = "name",
                    IsAscending = false,
                    Collation = "i;unicode-casemap"
                }
            ],
            Position = new JmapInt(0),
            Limit = new JmapUnsignedInt(25),
            CalculateTotal = true
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-query-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarQueryRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-query-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.Limit!.Value.Value).IsEqualTo(25L);
        await Assert.That(deserialized.CalculateTotal).IsTrue();
        await Assert.That(deserialized.Filter).IsTypeOf<JsonElement>();
    }

    [Test]
    public async Task CalendarQueryChangesRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new CalendarQueryChangesRequest
        {
            AccountId = new JmapId("account1"),
            Filter = new CalendarFilterCondition
            {
                Name = "Team",
                IsSubscribed = true
            },
            Sort =
            [
                new Comparator
                {
                    Property = "name"
                }
            ],
            SinceQueryState = "query-state-1",
            MaxChanges = new JmapUnsignedInt(10),
            UpToId = new JmapId("cal2"),
            CalculateTotal = true
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-query-changes-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarQueryChangesRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-query-changes-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.SinceQueryState).IsEqualTo("query-state-1");
        await Assert.That(deserialized.MaxChanges!.Value.Value).IsEqualTo(10L);
        await Assert.That(deserialized.UpToId).IsEqualTo(new JmapId("cal2"));
        await Assert.That(deserialized.Filter).IsTypeOf<JsonElement>();
    }

    [Test]
    public async Task CalendarChangesRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new CalendarChangesRequest
        {
            AccountId = new JmapId("account1"),
            SinceState = "state-1",
            MaxChanges = new JmapUnsignedInt(20)
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-changes-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarChangesRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-changes-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.SinceState).IsEqualTo("state-1");
        await Assert.That(deserialized.MaxChanges!.Value.Value).IsEqualTo(20L);
    }

    [Test]
    public async Task CalendarCopyRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new CalendarCopyRequest
        {
            FromAccountId = new JmapId("source-account"),
            IfFromInState = "source-state-1",
            AccountId = new JmapId("target-account"),
            IfInState = "target-state-1",
            Create = new Dictionary<JmapId, Calendar>
            {
                [new JmapId("copy-cal-1")] = CalendarFixtures.CreateCalendar()
            },
            OnSuccessDestroyOriginal = true,
            DestroyFromIfInState = "source-state-2"
        };

        var json = JsonSerializer.Serialize(request);
        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-copy-request.json"));

        var deserialized = JsonSerializer.Deserialize<CalendarCopyRequest>(
            await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-copy-request.json")));

        await Assert.That(deserialized).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(deserialized!.FromAccountId).IsEqualTo(new JmapId("source-account"));
        await Assert.That(deserialized.Create.ContainsKey(new JmapId("copy-cal-1"))).IsTrue();
        await Assert.That(deserialized.OnSuccessDestroyOriginal).IsTrue();
        await Assert.That(deserialized.DestroyFromIfInState).IsEqualTo("source-state-2");
    }
}
