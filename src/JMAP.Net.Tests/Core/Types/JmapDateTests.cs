using System.Text.Json;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Tests.Core.Types;

public class JmapDateTests
{
    [Test]
    public async Task Serialize_ShouldNormalizeJmapDate()
    {
        var value = new JmapDate(new DateTimeOffset(2025, 1, 2, 3, 4, 5, TimeSpan.FromHours(2)));

        var json = JsonSerializer.Serialize(value);

        await Assert.That(JsonSerializer.Deserialize<string>(json)).IsEqualTo("2025-01-02T03:04:05+02:00");
    }

    [Test]
    public async Task Deserialize_ShouldReadJmapDate()
    {
        var value = JsonSerializer.Deserialize<JmapDate>("\"2025-01-02T03:04:05+02:00\"");

        await Assert.That(value.Value).IsEqualTo(new DateTimeOffset(2025, 1, 2, 3, 4, 5, TimeSpan.FromHours(2)));
    }

    [Test]
    public async Task Serialize_ShouldNormalizeJmapUtcDate()
    {
        var value = new JmapUtcDate(new DateTimeOffset(2025, 1, 2, 3, 4, 5, TimeSpan.FromHours(2)));

        var json = JsonSerializer.Serialize(value);

        await Assert.That(JsonSerializer.Deserialize<string>(json)).IsEqualTo("2025-01-02T01:04:05Z");
    }

    [Test]
    public async Task Deserialize_ShouldRejectNonUtcDate()
    {
        var act = () => JsonSerializer.Deserialize<JmapUtcDate>("\"2025-01-02T03:04:05+02:00\"");

        await Assert.That(act).Throws<JsonException>();
    }
}
