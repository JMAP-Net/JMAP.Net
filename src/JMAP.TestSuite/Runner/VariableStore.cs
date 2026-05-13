namespace JMAP.TestSuite.Runner;

public sealed class VariableStore
{
    private readonly Dictionary<string, string> _values = new(StringComparer.Ordinal);

    public void Set(string name, string value) => _values[name] = value;

    public bool TryGet(string name, out string value) => _values.TryGetValue(name, out value!);
}
