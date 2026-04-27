using System.Text.Json;
using JMAP.Net.Capabilities.Core.Methods;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Tests.Common.Fixtures;

namespace JMAP.Net.Tests.Core.Methods;

public class BlobTransferJsonTests
{
    [Test]
    public async Task BlobUploadResponse_WhenSerialized_ShouldMatchFixture()
    {
        var response = new BlobUploadResponse
        {
            AccountId = new JmapId("account1"),
            BlobId = new JmapId("blob1"),
            Type = "text/calendar",
            Size = new JmapUnsignedInt(42)
        };

        var json = JsonSerializer.Serialize(response);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Protocol", "blob-upload-response.json"));
    }

    [Test]
    public async Task BlobUploadResponse_WhenDeserializedFromFixture_ShouldReadFields()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Protocol", "blob-upload-response.json"));

        var response = JsonSerializer.Deserialize<BlobUploadResponse>(json);

        await Assert.That(response).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(response!.AccountId).IsEqualTo(new JmapId("account1"));
        await Assert.That(response.BlobId).IsEqualTo(new JmapId("blob1"));
        await Assert.That(response.Type).IsEqualTo("text/calendar");
        await Assert.That(response.Size.Value).IsEqualTo(42L);
    }

    [Test]
    public async Task BlobDownloadRequest_WhenSerialized_ShouldMatchFixture()
    {
        var request = new BlobDownloadRequest
        {
            AccountId = new JmapId("account1"),
            BlobId = new JmapId("blob1"),
            Type = "text/calendar",
            Name = "event.ics"
        };

        var json = JsonSerializer.Serialize(request);

        await JsonAssert.AreEqualAsync(json, Path.Combine("Protocol", "blob-download-request.json"));
    }

    [Test]
    public async Task BlobDownloadRequest_WhenDeserializedFromFixture_ShouldReadTemplateValues()
    {
        var json = await FixtureLoader.LoadTextAsync(Path.Combine("Protocol", "blob-download-request.json"));

        var request = JsonSerializer.Deserialize<BlobDownloadRequest>(json);

        await Assert.That(request).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(request!.AccountId).IsEqualTo(new JmapId("account1"));
        await Assert.That(request.BlobId).IsEqualTo(new JmapId("blob1"));
        await Assert.That(request.Type).IsEqualTo("text/calendar");
        await Assert.That(request.Name).IsEqualTo("event.ics");
    }
}
