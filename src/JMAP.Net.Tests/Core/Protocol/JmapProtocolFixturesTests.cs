using System.Text.Json;
using JMAP.Net.Capabilities.Calendars;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Capabilities.Sharing;
using JMAP.Net.Common.Protocol;
using JMAP.Net.Common.Session;
using JMAP.Net.Tests.Common.Fixtures;

namespace JMAP.Net.Tests.Core.Protocol;

public class JmapProtocolFixturesTests
{
    [Test]
    public async Task RequestWithResultReference_WhenSerialized_ShouldMatchFixture()
    {
        var request = new JmapRequest
        {
            Using = ["urn:ietf:params:jmap:core"],
            MethodCalls =
            [
                new Invocation
                {
                    Name = "Item/query",
                    Arguments = new Dictionary<string, object?>
                    {
                        ["accountId"] = "account1",
                        ["filter"] = new Dictionary<string, object?> { ["inCategory"] = "default" }
                    },
                    MethodCallId = "c1"
                },
                new Invocation
                {
                    Name = "Item/get",
                    Arguments = new Dictionary<string, object?>
                    {
                        ["accountId"] = "account1",
                        ["#ids"] = new ResultReference
                        {
                            ResultOf = "c1",
                            Name = "Item/query",
                            Path = "/ids"
                        },
                        ["properties"] = new[] { "id", "name" }
                    },
                    MethodCallId = "c2"
                }
            ],
            CreatedIds = new Dictionary<JmapId, JmapId>
            {
                [new JmapId("temp1")] = new JmapId("item1")
            }
        };

        var json = JsonSerializer.Serialize(request);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Protocol", "jmap-request-with-result-reference.json"));
    }

    [Test]
    public async Task Request_WhenDeserializedFromFixture_ShouldExposeResultReferencePayload()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Protocol", "jmap-request-with-result-reference.json"));

        var request = JsonSerializer.Deserialize<JmapRequest>(json);

        await Assert.That(request).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(request!.Using.Count).IsEqualTo(1);
        await Assert.That(request.MethodCalls.Count).IsEqualTo(2);
        await Assert.That(request.MethodCalls[1].Arguments["#ids"]).IsTypeOf<JsonElement>();
        await Assert.That(((JsonElement)request.MethodCalls[1].Arguments["#ids"]!).GetProperty("resultOf").GetString())
            .IsEqualTo("c1");
    }

    [Test]
    public async Task SessionWithAccounts_WhenSerialized_ShouldMatchFixture()
    {
        var session = new JmapSession
        {
            Capabilities = new Dictionary<string, object>
            {
                ["urn:ietf:params:jmap:core"] = new Dictionary<string, object>
                {
                    ["maxSizeUpload"] = 50000000
                }
            },
            Accounts = new Dictionary<JmapId, JmapAccount>
            {
                [new JmapId("account1")] = new()
                {
                    Name = "user@example.com",
                    IsPersonal = true,
                    IsReadOnly = false,
                    AccountCapabilities = new Dictionary<string, object>
                    {
                        ["urn:ietf:params:jmap:core"] = new Dictionary<string, object>()
                    }
                }
            },
            PrimaryAccounts = new Dictionary<string, JmapId>
            {
                ["urn:ietf:params:jmap:core"] = new JmapId("account1")
            },
            Username = "user@example.com",
            ApiUrl = "https://jmap.example.com/api/",
            DownloadUrl = "https://jmap.example.com/download/{accountId}/{blobId}/{name}?type={type}",
            UploadUrl = "https://jmap.example.com/upload/{accountId}/",
            EventSourceUrl = "https://jmap.example.com/eventsource/?types={types}&closeafter={closeafter}&ping={ping}",
            State = "state-1"
        };

        var json = JsonSerializer.Serialize(session);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Protocol", "jmap-session-with-accounts.json"));
    }

    [Test]
    public async Task Session_WhenDeserializedFromFixture_ShouldReadJmapIdDictionaryKeys()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Protocol", "jmap-session-with-accounts.json"));

        var session = JsonSerializer.Deserialize<JmapSession>(json);

        await Assert.That(session).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(session!.Accounts.ContainsKey(new JmapId("account1"))).IsTrue();
        await Assert.That(session.PrimaryAccounts["urn:ietf:params:jmap:core"]).IsEqualTo(new JmapId("account1"));
    }

    [Test]
    public async Task SessionWithCalendarCapabilities_WhenDeserializedFromFixture_ShouldReadCapabilityPlacement()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Protocol", "jmap-session-with-calendar-capabilities.json"));

        var session = JsonSerializer.Deserialize<JmapSession>(json);

        await Assert.That(session).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(session!.Capabilities.ContainsKey(CalendarCapability.CapabilityUri)).IsTrue();
        await Assert.That(session.Capabilities.ContainsKey(CalendarParseCapability.CapabilityUri)).IsTrue();
        await Assert.That(session.Capabilities.ContainsKey(PrincipalCapability.CapabilityUri)).IsTrue();
        await Assert.That(session.Capabilities.ContainsKey(PrincipalAvailabilityCapability.CapabilityUri)).IsTrue();
        await Assert.That(session.Accounts[new JmapId("account1")].AccountCapabilities.ContainsKey(CalendarAccountCapability.CapabilityUri)).IsTrue();
        await Assert.That(session.Accounts[new JmapId("account1")].AccountCapabilities.ContainsKey(CalendarParseCapability.CapabilityUri)).IsTrue();
        await Assert.That(session.Accounts[new JmapId("account1")].AccountCapabilities.ContainsKey(PrincipalAccountCapability.CapabilityUri)).IsTrue();
        await Assert.That(session.Accounts[new JmapId("account1")].AccountCapabilities.ContainsKey(PrincipalOwnerCapability.CapabilityUri)).IsTrue();
        await Assert.That(session.Accounts[new JmapId("account1")].AccountCapabilities.ContainsKey(PrincipalAvailabilityAccountCapability.CapabilityUri)).IsTrue();
        await Assert.That(session.PrimaryAccounts.ContainsKey("urn:ietf:params:jmap:core")).IsFalse();
        await Assert.That(session.PrimaryAccounts[CalendarCapability.CapabilityUri]).IsEqualTo(new JmapId("account1"));
    }
}
