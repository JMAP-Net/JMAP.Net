using System.Text.Json;
using JMAP.Net.Common.Errors;
using JMAP.Net.Tests.Common.Fixtures;

namespace JMAP.Net.Tests.Core.Errors;

public class JmapErrorJsonTests
{
    [Test]
    public async Task JmapErrorWithExtensionData_WhenSerialized_ShouldMatchFixture()
    {
        var error = new JmapError
        {
            Type = JmapErrorType.InvalidArguments,
            Description = "The 'ids' argument is invalid.",
            ExtensionData = new Dictionary<string, object?>
            {
                ["arguments"] = new[] { "ids" }
            }
        };

        var json = JsonSerializer.Serialize(error);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Protocol", "jmap-error-invalid-arguments.json"));
    }

    [Test]
    public async Task JmapError_WhenDeserializedFromFixture_ShouldReadExtensionData()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Protocol", "jmap-error-invalid-arguments.json"));

        var error = JsonSerializer.Deserialize<JmapError>(json);

        await Assert.That(error).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(error!.Type).IsEqualTo(JmapErrorType.InvalidArguments);
        await Assert.That(error.Description).IsEqualTo("The 'ids' argument is invalid.");
        await Assert.That(error.ExtensionData).IsNotNull();
        await Assert.That(error.ExtensionData!["arguments"]).IsTypeOf<JsonElement>();
        await Assert.That(((JsonElement)error.ExtensionData["arguments"]!).GetArrayLength()).IsEqualTo(1);
    }
}
