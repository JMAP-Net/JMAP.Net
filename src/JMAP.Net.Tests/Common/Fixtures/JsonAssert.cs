using System.Text.Json;

namespace JMAP.Net.Tests.Common.Fixtures;

internal static class JsonAssert
{
    public static async Task AreEqualAsync(string actualJson, string fixtureRelativePath)
    {
        var expected = await FixtureLoader.LoadJsonAsync(fixtureRelativePath);
        using var actualDocument = JsonDocument.Parse(actualJson);
        var isEqual = JsonElement.DeepEquals(actualDocument.RootElement, expected);

        if (!isEqual)
        {
            throw new InvalidOperationException(
                $"JSON did not match fixture '{fixtureRelativePath}'.\nActual: {actualJson}");
        }
    }
}
