using System.Text.Json;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Tests.Protocol;

public class ResultReferenceJsonConverterTests
{
    [Test]
    public async Task Serialize_ShouldWriteObjectNotation()
    {
        var reference = new ResultReference
        {
            ResultOf = "c1",
            Name = "Item/query",
            Path = "/ids"
        };

        var json = JsonSerializer.Serialize(reference);

        await Assert.That(json).IsEqualTo("""{"resultOf":"c1","name":"Item/query","path":"/ids"}""");
    }

    [Test]
    public async Task Deserialize_ShouldReadObjectNotation()
    {
        const string json = """{"resultOf":"c1","name":"Item/query","path":"/ids"}""";

        var reference = JsonSerializer.Deserialize<ResultReference>(json);

        await Assert.That(reference).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(reference!.ResultOf).IsEqualTo("c1");
        await Assert.That(reference.Name).IsEqualTo("Item/query");
        await Assert.That(reference.Path).IsEqualTo("/ids");
    }

    [Test]
    public async Task Deserialize_ShouldRejectMissingRequiredProperties()
    {
        const string json = """{"resultOf":"c1","name":"Item/query"}""";
        var act = () => JsonSerializer.Deserialize<ResultReference>(json);

        await Assert.That(act).Throws<JsonException>();
    }
}
