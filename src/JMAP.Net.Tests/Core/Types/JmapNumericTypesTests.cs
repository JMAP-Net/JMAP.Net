using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Tests.Core.Types;

public class JmapNumericTypesTests
{
    [Test]
    public async Task JmapInt_ShouldAcceptSafeIntegerRange()
    {
        var value = new JmapInt(9007199254740991L);

        await Assert.That(value.Value).IsEqualTo(9007199254740991L);
    }

    [Test]
    public async Task JmapInt_ShouldRejectValuesOutsideSafeIntegerRange()
    {
        var act = () => new JmapInt(9007199254740992L);

        await Assert.That(act).Throws<ArgumentOutOfRangeException>()
            .WithParameterName("value");
    }

    [Test]
    public async Task JmapUnsignedInt_ShouldAcceptZero()
    {
        var value = new JmapUnsignedInt(0);

        await Assert.That(value.Value).IsEqualTo(0L);
    }

    [Test]
    public async Task JmapUnsignedInt_ShouldRejectNegativeValues()
    {
        var act = () => new JmapUnsignedInt(-1);

        await Assert.That(act).Throws<ArgumentOutOfRangeException>()
            .WithParameterName("value");
    }
}
