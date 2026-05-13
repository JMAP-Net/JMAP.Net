using System.Text.Json.Nodes;

namespace JMAP.TestSuite.Runner.Assertions;

internal static class JsonPath
{
    public static bool TryResolve(JsonNode? root, string path, out JsonNode? value)
    {
        value = root;

        if (root is null || string.IsNullOrWhiteSpace(path) || path[0] != '$')
        {
            return false;
        }

        var index = 1;
        while (index < path.Length)
        {
            if (path[index] == '.')
            {
                index++;
                var start = index;
                while (index < path.Length && path[index] is not '.' and not '[')
                {
                    index++;
                }

                var property = path[start..index];
                if (value is not JsonObject obj || !obj.TryGetPropertyValue(property, out value))
                {
                    return false;
                }
            }
            else if (path[index] == '[')
            {
                index++;
                var start = index;
                while (index < path.Length && path[index] != ']')
                {
                    index++;
                }

                if (index >= path.Length || !int.TryParse(path[start..index], out var arrayIndex))
                {
                    return false;
                }

                index++;

                if (value is not JsonArray array || arrayIndex < 0 || arrayIndex >= array.Count)
                {
                    return false;
                }

                value = array[arrayIndex];
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}
