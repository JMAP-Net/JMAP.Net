using System.Text.Json;
using JMAP.Net.Common.Errors;
using JMAP.Net.Tests.Common.Fixtures;

namespace JMAP.Net.Tests.Core.Errors;

public class ProblemDetailsJsonTests
{
    [Test]
    public async Task ProblemDetailsWithExtensionData_WhenSerialized_ShouldMatchFixture()
    {
        var details = new ProblemDetails
        {
            Type = ProblemDetailsType.Limit,
            Status = 429,
            Detail = "The request exceeded the maximum number of method calls.",
            Limit = "maxCallsInRequest",
            ExtensionData = new Dictionary<string, object?>
            {
                ["maxCallsInRequest"] = 16
            }
        };

        var json = JsonSerializer.Serialize(details);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Protocol", "problem-details-limit.json"));
    }

    [Test]
    public async Task ProblemDetails_WhenDeserializedFromFixture_ShouldReadExtensionData()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Protocol", "problem-details-limit.json"));

        var details = JsonSerializer.Deserialize<ProblemDetails>(json);

        await Assert.That(details).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(details!.Type).IsEqualTo(ProblemDetailsType.Limit);
        await Assert.That(details.Status).IsEqualTo(429);
        await Assert.That(details.Limit).IsEqualTo("maxCallsInRequest");
        await Assert.That(details.ExtensionData).IsNotNull();
        await Assert.That(details.ExtensionData!["maxCallsInRequest"]).IsTypeOf<JsonElement>();
        await Assert.That(((JsonElement)details.ExtensionData["maxCallsInRequest"]!).GetInt32()).IsEqualTo(16);
    }
}
