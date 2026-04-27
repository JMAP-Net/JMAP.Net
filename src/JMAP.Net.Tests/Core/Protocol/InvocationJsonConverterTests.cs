using System.Text.Json;
using JMAP.Net.Common.Protocol;

namespace JMAP.Net.Tests.Core.Protocol;

public class InvocationJsonConverterTests
{
    [Test]
    public async Task Serialize_ShouldWriteInvocationAsJsonArray()
    {
        var invocation = new Invocation
        {
            Name = "Core/echo",
            Arguments = new Dictionary<string, object?>
            {
                ["hello"] = "world",
                ["count"] = 2
            },
            MethodCallId = "call-1"
        };

        var json = JsonSerializer.Serialize(invocation);

        await Assert.That(json).IsEqualTo("""["Core/echo",{"hello":"world","count":2},"call-1"]""");
    }

    [Test]
    public async Task Deserialize_ShouldReadInvocationFromJsonArray()
    {
        const string json = """["Core/echo",{"hello":"world","count":2},"call-1"]""";

        var invocation = JsonSerializer.Deserialize<Invocation>(json);

        await Assert.That(invocation).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(invocation!.Name).IsEqualTo("Core/echo");
        await Assert.That(invocation.MethodCallId).IsEqualTo("call-1");
        await Assert.That(invocation.Arguments["hello"]).IsTypeOf<JsonElement>();
        await Assert.That(((JsonElement)invocation.Arguments["hello"]!).GetString()).IsEqualTo("world");
        await Assert.That(((JsonElement)invocation.Arguments["count"]!).GetInt32()).IsEqualTo(2);
    }
}
