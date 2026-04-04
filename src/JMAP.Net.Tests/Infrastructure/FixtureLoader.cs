using System.Text.Json;

namespace JMAP.Net.Tests.Infrastructure;

internal static class FixtureLoader
{
    public static async Task<string> LoadTextAsync(string relativePath)
    {
        var fullPath = Path.Combine(AppContext.BaseDirectory, "Fixtures", relativePath);
        return await File.ReadAllTextAsync(fullPath);
    }

    public static async Task<JsonElement> LoadJsonAsync(string relativePath)
    {
        var json = await LoadTextAsync(relativePath);
        using var document = JsonDocument.Parse(json);
        return document.RootElement.Clone();
    }
}
