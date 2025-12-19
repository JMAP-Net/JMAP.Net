using System.Text.Json.Serialization;
using JMAP.Net.Common.Converters;

namespace JMAP.Net.Capabilities.Core.Types;

/// <summary>
/// Represents a JMAP UnsignedInt type - an integer in the range 0 to 2^53-1.
/// As per RFC 8620, Section 1.3.
/// </summary>
[JsonConverter(typeof(JmapUnsignedIntJsonConverter))]
public readonly struct JmapUnsignedInt : IEquatable<JmapUnsignedInt>, IComparable<JmapUnsignedInt>
{
    private const long MinValue = 0L;
    private const long MaxValue = 9007199254740991L; // 2^53-1

    public long Value { get; }

    public JmapUnsignedInt(long value)
    {
        if (value is < MinValue or > MaxValue)
            throw new ArgumentOutOfRangeException(
                nameof(value),
                $"JMAP UnsignedInt must be in the range {MinValue} to {MaxValue}");

        Value = value;
    }

    public static implicit operator long(JmapUnsignedInt value) => value.Value;
    public static explicit operator JmapUnsignedInt(long value) => new(value);
    public static explicit operator JmapUnsignedInt(int value) => new(value);
    public static explicit operator JmapUnsignedInt(uint value) => new(value);

    public override string ToString() => Value.ToString();
    public bool Equals(JmapUnsignedInt other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is JmapUnsignedInt other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();

    public int CompareTo(JmapUnsignedInt other) => Value.CompareTo(other.Value);

    public static bool operator ==(JmapUnsignedInt left, JmapUnsignedInt right) => left.Equals(right);
    public static bool operator !=(JmapUnsignedInt left, JmapUnsignedInt right) => !left.Equals(right);
    public static bool operator <(JmapUnsignedInt left, JmapUnsignedInt right) => left.CompareTo(right) < 0;
    public static bool operator >(JmapUnsignedInt left, JmapUnsignedInt right) => left.CompareTo(right) > 0;
    public static bool operator <=(JmapUnsignedInt left, JmapUnsignedInt right) => left.CompareTo(right) <= 0;
    public static bool operator >=(JmapUnsignedInt left, JmapUnsignedInt right) => left.CompareTo(right) >= 0;
}