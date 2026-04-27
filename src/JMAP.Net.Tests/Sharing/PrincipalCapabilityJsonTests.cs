using System.Text.Json;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Capabilities.Sharing;
using JMAP.Net.Tests.Common.Fixtures;

namespace JMAP.Net.Tests.Sharing;

public class PrincipalCapabilityJsonTests
{
    [Test]
    public async Task PrincipalCapability_WhenSerialized_ShouldBeEmptyObject()
    {
        var capability = new PrincipalCapability();

        var json = JsonSerializer.Serialize(capability);

        await Assert.That(json).IsEqualTo("{}");
        await Assert.That(PrincipalCapability.CapabilityUri).IsEqualTo("urn:ietf:params:jmap:principals");
    }

    [Test]
    public async Task PrincipalAccountCapability_WhenSerialized_ShouldWriteCurrentUserPrincipalId()
    {
        var capability = new PrincipalAccountCapability
        {
            CurrentUserPrincipalId = new JmapId("principal1")
        };

        var json = JsonSerializer.Serialize(capability);

        await Assert.That(json).IsEqualTo("""{"currentUserPrincipalId":"principal1"}""");
        await Assert.That(PrincipalAccountCapability.CapabilityUri).IsEqualTo(PrincipalCapability.CapabilityUri);
    }

    [Test]
    public async Task PrincipalOwnerCapability_WhenSerialized_ShouldWriteOwnerProperties()
    {
        var capability = new PrincipalOwnerCapability
        {
            AccountIdForPrincipal = new JmapId("principal-account"),
            PrincipalId = new JmapId("principal1")
        };

        var json = JsonSerializer.Serialize(capability);

        using var actual = JsonDocument.Parse(json);
        using var expected = JsonDocument.Parse(
            """
            {
              "accountIdForPrincipal": "principal-account",
              "principalId": "principal1"
            }
            """);

        await Assert.That(JsonElement.DeepEquals(actual.RootElement, expected.RootElement)).IsTrue();
        await Assert.That(PrincipalOwnerCapability.CapabilityUri)
            .IsEqualTo("urn:ietf:params:jmap:principals:owner");
    }
}
