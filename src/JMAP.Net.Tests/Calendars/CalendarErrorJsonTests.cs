using System.Text.Json;
using JMAP.Net.Common.Errors;
using JMAP.Net.Tests.Common.Fixtures;

namespace JMAP.Net.Tests.Calendars;

public class CalendarErrorJsonTests
{
    [Test]
    public async Task CalendarSetError_WhenSerialized_ShouldMatchFixture()
    {
        var error = new SetError
        {
            Type = SetErrorType.CalendarHasEvent,
            Description = "Calendar cannot be destroyed while it still contains events"
        };

        var json = JsonSerializer.Serialize(error);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-set-error-calendar-has-event.json"));
    }

    [Test]
    public async Task CalendarMethodErrors_WhenSerialized_ShouldMatchFixture()
    {
        var errors = new List<JmapError>
        {
            new()
            {
                Type = JmapErrorType.NoSupportedScheduleMethods,
                Description = "The account has no participant identities with a supported scheduling method"
            },
            new()
            {
                Type = JmapErrorType.ExpandDurationTooLarge,
                Description = "The requested recurrence expansion duration is too large",
                ExtensionData = new Dictionary<string, object?>
                {
                    ["maxDuration"] = "P1Y"
                }
            },
            new()
            {
                Type = JmapErrorType.CannotCalculateOccurrences,
                Description = "Occurrences cannot be calculated for the supplied recurrence data",
                ExtensionData = new Dictionary<string, object?>
                {
                    ["eventId"] = "event1"
                }
            }
        };

        var json = JsonSerializer.Serialize(errors);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Calendars", "calendar-method-errors.json"));
    }

    [Test]
    public async Task CalendarMethodErrors_WhenDeserializedFromFixture_ShouldReadTypesAndExtensionData()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Calendars", "calendar-method-errors.json"));

        var errors = JsonSerializer.Deserialize<List<JmapError>>(json);

        await Assert.That(errors).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(errors!.Count).IsEqualTo(3);
        await Assert.That(errors[0].Type).IsEqualTo(JmapErrorType.NoSupportedScheduleMethods);
        await Assert.That(errors[1].Type).IsEqualTo(JmapErrorType.ExpandDurationTooLarge);
        await Assert.That(((JsonElement)errors[1].ExtensionData!["maxDuration"]!).GetString()).IsEqualTo("P1Y");
        await Assert.That(errors[2].Type).IsEqualTo(JmapErrorType.CannotCalculateOccurrences);
        await Assert.That(((JsonElement)errors[2].ExtensionData!["eventId"]!).GetString()).IsEqualTo("event1");
    }
}
