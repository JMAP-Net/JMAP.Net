using System.Text.Json;
using JMAP.Net.Capabilities.Core.Methods;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Common.Errors;
using JMAP.Net.Common.Protocol;
using JMAP.Net.Tests.Infrastructure;

namespace JMAP.Net.Tests.Protocol;

public class JmapMethodResponsesTests
{
    private sealed class TestGetResponse : GetResponse<JsonElement>
    {
    }

    private sealed class TestSetResponse : SetResponse<JsonElement>
    {
    }

    [Test]
    public async Task Serialize_ResponseWithCreatedIds_ShouldMatchFixture()
    {
        var response = new JmapResponse
        {
            MethodResponses =
            [
                new Invocation
                {
                    Name = "Item/set",
                    Arguments = new Dictionary<string, object?>
                    {
                        ["accountId"] = "account1",
                        ["oldState"] = "state-1",
                        ["newState"] = "state-2",
                        ["created"] = new Dictionary<string, object?>
                        {
                            ["temp1"] = new Dictionary<string, object?>
                            {
                                ["id"] = "item1",
                                ["updatedAt"] = "2026-03-29T12:00:00Z"
                            }
                        },
                        ["notCreated"] = new Dictionary<string, object?>
                        {
                            ["temp2"] = new Dictionary<string, object?>
                            {
                                ["type"] = SetErrorType.InvalidProperties,
                                ["description"] = "name is required",
                                ["properties"] = new[] { "name" }
                            }
                        }
                    },
                    MethodCallId = "c1"
                }
            ],
            CreatedIds = new Dictionary<JmapId, JmapId>
            {
                [new JmapId("temp1")] = new JmapId("item1")
            },
            SessionState = "session-1"
        };

        var json = JsonSerializer.Serialize(response);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Protocol", "jmap-response-with-created-ids.json"));
    }

    [Test]
    public async Task Deserialize_GetResponseFixture_ShouldReadIdsAndNotFound()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Protocol", "get-response-minimal.json"));

        var response = JsonSerializer.Deserialize<TestGetResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.AccountId).IsEqualTo(new JmapId("account1"));
        await Assert.That(response.State).IsEqualTo("state-1");
        await Assert.That(response.NotFound.Count).IsEqualTo(1);
        await Assert.That(response.NotFound[0]).IsEqualTo(new JmapId("missing1"));
        await Assert.That(response.List[0].GetProperty("id").GetString()).IsEqualTo("item1");
    }

    [Test]
    public async Task Deserialize_SetResponseFixture_ShouldReadSuccessAndErrorBuckets()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Protocol", "set-response-with-errors.json"));

        var response = JsonSerializer.Deserialize<TestSetResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.AccountId).IsEqualTo(new JmapId("account1"));
        await Assert.That(response.Created!.ContainsKey(new JmapId("temp1"))).IsTrue();
        await Assert.That(response.Updated!.ContainsKey(new JmapId("item2"))).IsTrue();
        await Assert.That(response.Destroyed!.Count).IsEqualTo(1);
        await Assert.That(response.NotCreated![new JmapId("temp2")].Type).IsEqualTo(SetErrorType.InvalidProperties);
        await Assert.That(response.NotUpdated![new JmapId("item4")].Type).IsEqualTo(SetErrorType.NotFound);
        await Assert.That(response.NotDestroyed![new JmapId("item5")].Type).IsEqualTo(SetErrorType.Forbidden);
    }
}
