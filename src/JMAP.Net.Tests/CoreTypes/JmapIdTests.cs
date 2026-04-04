using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Tests.CoreTypes;

public class JmapIdTests
{
    [Test]
    public async Task Constructor_ShouldAcceptValidId()
    {
        var id = new JmapId("account_123-test");

        await Assert.That(id.Value).IsEqualTo("account_123-test");
    }

    [Test]
    public async Task Constructor_ShouldRejectInvalidCharacters()
    {
        var act = () => new JmapId("invalid id");

        await Assert.That(act).Throws<ArgumentException>();
    }

    [Test]
    public async Task IsValid_ShouldReturnExpectedResults()
    {
        using var _ = Assert.Multiple();
        await Assert.That(JmapId.IsValid("valid_Id-1")).IsTrue();
        await Assert.That(JmapId.IsValid(string.Empty)).IsFalse();
        await Assert.That(JmapId.IsValid("contains space")).IsFalse();
    }

    [Test]
    public async Task TryParse_ShouldReturnFalseForInvalidValue()
    {
        var parsed = JmapId.TryParse("contains space", out var id);

        using var _ = Assert.Multiple();
        await Assert.That(parsed).IsFalse();
        await Assert.That(id.Value).IsNull();
    }

    [Test]
    public async Task FollowsBestPractices_ShouldRejectProblematicPatterns()
    {
        using var _ = Assert.Multiple();
        await Assert.That(new JmapId("-abc").FollowsBestPractices()).IsFalse();
        await Assert.That(new JmapId("123").FollowsBestPractices()).IsFalse();
        await Assert.That(new JmapId("NIL").FollowsBestPractices()).IsFalse();
        await Assert.That(new JmapId("abc123").FollowsBestPractices()).IsTrue();
    }
}
