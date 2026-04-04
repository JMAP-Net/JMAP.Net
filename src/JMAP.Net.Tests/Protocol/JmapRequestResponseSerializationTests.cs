using System.Text.Json;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Common.Protocol;

namespace JMAP.Net.Tests.Protocol;

public class JmapRequestResponseSerializationTests
{
    [Test]
    public async Task Serialize_ShouldIncludeCreatedIdsForRequest()
    {
        var request = new JmapRequest
        {
            Using = ["urn:ietf:params:jmap:core"],
            MethodCalls =
            [
                new Invocation
                {
                    Name = "Core/echo",
                    Arguments = new Dictionary<string, object?> { ["hello"] = "world" },
                    MethodCallId = "c1"
                }
            ],
            CreatedIds = new Dictionary<JmapId, JmapId>
            {
                [new JmapId("temp1")] = new JmapId("real1")
            }
        };

        var json = JsonSerializer.Serialize(request);

        using var _ = Assert.Multiple();
        await Assert.That(json).Contains("\"createdIds\":{\"temp1\":\"real1\"}");
        await Assert.That(json).Contains("\"methodCalls\":[[\"Core/echo\"");
    }

    [Test]
    public async Task Deserialize_ShouldReadCreatedIdsForResponse()
    {
        const string json =
            """
            {
              "methodResponses":[["Core/echo",{"hello":"world"},"c1"]],
              "createdIds":{"temp1":"real1"},
              "sessionState":"state-1"
            }
            """;

        var response = JsonSerializer.Deserialize<JmapResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.SessionState).IsEqualTo("state-1");
        await Assert.That(response.CreatedIds).IsNotNull();
        await Assert.That(response.CreatedIds!).ContainsKey(new JmapId("temp1"));
        await Assert.That(response.CreatedIds[new JmapId("temp1")]).IsEqualTo(new JmapId("real1"));
        await Assert.That(response.MethodResponses.Count).IsEqualTo(1);
        await Assert.That(response.MethodResponses[0].Name).IsEqualTo("Core/echo");
    }
}
