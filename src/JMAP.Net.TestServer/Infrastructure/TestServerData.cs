using System.Text.Json.Nodes;

namespace JMAP.Net.TestServer.Infrastructure;

public sealed class TestServerData
{
    public const string AccountId = "account1";
    public const string UserPrincipalId = "principal-user";
    public const string CalendarId = "calendar-work";
    public const string EventId = "event-standup";

    public string CalendarState0 { get; } = "calendar-state-0";

    public string CalendarState { get; } = "calendar-state-1";

    public string CalendarEventState0 { get; } = "calendar-event-state-0";

    public string CalendarEventState { get; } = "calendar-event-state-1";

    public JsonObject UserPrincipal { get; } = new()
    {
        ["id"] = UserPrincipalId,
        ["type"] = "individual",
        ["name"] = "Test User",
        ["description"] = "Local JMAP.Net test user",
        ["email"] = "user@example.test",
        ["timeZone"] = "Europe/Berlin",
        ["capabilities"] = new JsonObject
        {
            ["urn:ietf:params:jmap:calendars"] = new JsonObject()
        }
    };

    public JsonObject WorkCalendar { get; } = new()
    {
        ["id"] = CalendarId,
        ["name"] = "Work",
        ["description"] = "Deterministic calendar for CLI tests",
        ["color"] = "#2f80ed",
        ["sortOrder"] = 10,
        ["isSubscribed"] = true,
        ["isVisible"] = true,
        ["isDefault"] = true,
        ["includeInAvailability"] = "all",
        ["timeZone"] = "Europe/Berlin",
        ["myRights"] = new JsonObject
        {
            ["mayReadFreeBusy"] = true,
            ["mayReadItems"] = true,
            ["mayWriteAll"] = true,
            ["mayWriteOwn"] = true,
            ["mayUpdatePrivate"] = true,
            ["mayRSVP"] = true,
            ["mayAdmin"] = true,
            ["mayDelete"] = true
        }
    };

    public JsonObject StandupEvent { get; } = new()
    {
        ["id"] = EventId,
        ["uid"] = "standup-2026@example.test",
        ["calendarIds"] = new JsonObject
        {
            [CalendarId] = true
        },
        ["title"] = "Daily Standup",
        ["description"] = "Deterministic event for CLI tests",
        ["start"] = "2026-05-13T09:00:00",
        ["duration"] = "PT30M",
        ["timeZone"] = "Europe/Berlin",
        ["showWithoutTime"] = false,
        ["freeBusyStatus"] = "busy",
        ["privacy"] = "public"
    };
}
