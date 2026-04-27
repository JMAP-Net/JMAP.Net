using System.Text.Json;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Common.Protocol;

namespace JMAP.Net.Hosting.Services;

internal sealed class JmapResultReferenceResolver : IJmapResultReferenceResolver
{
    public bool ContainsResultReference(Invocation invocation)
    {
        ArgumentNullException.ThrowIfNull(invocation);

        return invocation.Arguments.Keys.Any(key => key.StartsWith('#'))
            || invocation.Arguments.Values.Any(ContainsResultReference);
    }

    public bool TryResolve(
        Invocation invocation,
        IReadOnlyList<Invocation?> previousResponses,
        out Invocation resolvedInvocation)
    {
        ArgumentNullException.ThrowIfNull(invocation);
        ArgumentNullException.ThrowIfNull(previousResponses);

        var arguments = new Dictionary<string, object?>(StringComparer.Ordinal);

        foreach (var (key, value) in invocation.Arguments)
        {
            if (!key.StartsWith('#'))
            {
                arguments[key] = value;
                continue;
            }

            if (!TryReadResultReference(value, out var reference)
                || !TryResolveResultReference(reference, previousResponses, out var resolvedValue))
            {
                resolvedInvocation = invocation;
                return false;
            }

            arguments[key[1..]] = resolvedValue;
        }

        resolvedInvocation = new Invocation
        {
            Name = invocation.Name,
            Arguments = arguments,
            MethodCallId = invocation.MethodCallId
        };
        return true;
    }

    private static bool TryReadResultReference(object? value, out ResultReference reference)
    {
        if (value is ResultReference resultReference)
        {
            reference = resultReference;
            return true;
        }

        if (value is JsonElement element && element.ValueKind == JsonValueKind.Object)
        {
            try
            {
                var parsed = element.Deserialize<ResultReference>();
                if (parsed is not null)
                {
                    reference = parsed;
                    return true;
                }
            }
            catch (JsonException)
            {
            }
        }

        reference = null!;
        return false;
    }

    private static bool TryResolveResultReference(
        ResultReference reference,
        IReadOnlyList<Invocation?> previousResponses,
        out object? resolvedValue)
    {
        var response = previousResponses
            .TakeWhile(response => response is not null)
            .FirstOrDefault(response =>
                response!.MethodCallId == reference.ResultOf
                && response.Name == reference.Name);

        if (response is null)
        {
            resolvedValue = null;
            return false;
        }

        return TryEvaluateJsonPointer(response.Arguments, reference.Path, out resolvedValue);
    }

    private static bool TryEvaluateJsonPointer(object? value, string path, out object? result)
    {
        if (!path.StartsWith('/'))
        {
            result = null;
            return false;
        }

        return TryEvaluateJsonPointer(
            value,
            path.Split('/').Skip(1).Select(UnescapeJsonPointerToken).ToArray(),
            0,
            out result);
    }

    private static bool TryEvaluateJsonPointer(object? value, string[] tokens, int index, out object? result)
    {
        if (index == tokens.Length)
        {
            result = NormalizeJsonElement(value);
            return true;
        }

        var token = tokens[index];
        if (token == "*")
        {
            return TryEvaluateWildcard(value, tokens, index, out result);
        }

        if (TryAsDictionary(value, out var dictionary) && dictionary.TryGetValue(token, out var child))
        {
            return TryEvaluateJsonPointer(child, tokens, index + 1, out result);
        }

        if (TryAsList(value, out var array)
            && int.TryParse(token, out var arrayIndex)
            && arrayIndex >= 0
            && arrayIndex < array.Count)
        {
            return TryEvaluateJsonPointer(array[arrayIndex], tokens, index + 1, out result);
        }

        result = null;
        return false;
    }

    private static bool TryEvaluateWildcard(object? value, string[] tokens, int index, out object? result)
    {
        if (!TryAsList(value, out var list))
        {
            result = null;
            return false;
        }

        var values = new List<object?>();
        foreach (var item in list)
        {
            if (!TryEvaluateJsonPointer(item, tokens, index + 1, out var itemResult))
            {
                result = null;
                return false;
            }

            if (itemResult is IEnumerable<object?> nested && itemResult is not string)
            {
                values.AddRange(nested);
            }
            else
            {
                values.Add(itemResult);
            }
        }

        result = values;
        return true;
    }

    private static string UnescapeJsonPointerToken(string token)
        => token.Replace("~1", "/", StringComparison.Ordinal).Replace("~0", "~", StringComparison.Ordinal);

    private static bool TryAsDictionary(object? value, out IReadOnlyDictionary<string, object?> dictionary)
    {
        value = NormalizeJsonElement(value);

        if (value is IReadOnlyDictionary<string, object?> readOnlyDictionary)
        {
            dictionary = readOnlyDictionary;
            return true;
        }

        dictionary = null!;
        return false;
    }

    private static bool TryAsList(object? value, out IReadOnlyList<object?> list)
    {
        value = NormalizeJsonElement(value);

        if (value is IReadOnlyList<object?> readOnlyList)
        {
            list = readOnlyList;
            return true;
        }

        list = null!;
        return false;
    }

    private static object? NormalizeJsonElement(object? value)
    {
        return value is JsonElement element
            ? element.ValueKind switch
            {
                JsonValueKind.Object => element.EnumerateObject()
                    .ToDictionary(property => property.Name, property => NormalizeJsonElement(property.Value), StringComparer.Ordinal),
                JsonValueKind.Array => element.EnumerateArray().Select(item => NormalizeJsonElement(item)).ToList(),
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number when element.TryGetInt64(out var longValue) => longValue,
                JsonValueKind.Number when element.TryGetDouble(out var doubleValue) => doubleValue,
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => value
            }
            : value;
    }

    private static bool ContainsResultReference(object? value)
    {
        if (value is null)
        {
            return false;
        }

        if (value is IReadOnlyDictionary<string, object?> dictionary)
        {
            return dictionary.Keys.Any(key => key.StartsWith('#'))
                || dictionary.Values.Any(ContainsResultReference);
        }

        if (value is JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.Object => element.EnumerateObject().Any(property =>
                    property.Name.StartsWith('#') || ContainsResultReference(property.Value)),
                JsonValueKind.Array => element.EnumerateArray().Any(item => ContainsResultReference(item)),
                _ => false
            };
        }

        return value is not string
            && value is IEnumerable<object?> enumerable
            && enumerable.Any(ContainsResultReference);
    }
}
