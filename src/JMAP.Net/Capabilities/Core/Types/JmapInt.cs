using System.Text.Json.Serialization;
using JMAP.Net.Common.Converters;

namespace JMAP.Net.Capabilities.Core.Types;

/// <summary>
/// Represents a JMAP Int type - an integer in the range -2^53+1 to 2^53-1.
/// This is the safe range for integers stored in a floating-point double.
/// As per RFC 8620, Section 1.3.
/// </summary>
[JsonConverter(typeof(JmapIntJsonConverter))]
public readonly struct JmapInt : IEquatable<JmapInt>, IComparable<JmapInt>
{
    private const long MinValue = -9007199254740991L; // -(2^53-1)
    private const long MaxValue = 9007199254740991L; // 2^53-1

    public long Value { get; }

    public JmapInt(long value)
    {
        if (value is < MinValue or > MaxValue)
            throw new ArgumentOutOfRangeException(
                nameof(value),
                $"JMAP Int must be in the range {MinValue} to {MaxValue}");

        Value = value;
    }

    public static implicit operator long(JmapInt value) => value.Value;
    public static explicit operator JmapInt(long value) => new(value);
    public static explicit operator JmapInt(int value) => new(value);

    public override string ToString() => Value.ToString();
    public bool Equals(JmapInt other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is JmapInt other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();

    public int CompareTo(JmapInt other) => Value.CompareTo(other.Value);

    public static bool operator ==(JmapInt left, JmapInt right) => left.Equals(right);
    public static bool operator !=(JmapInt left, JmapInt right) => !left.Equals(right);
    public static bool operator <(JmapInt left, JmapInt right) => left.CompareTo(right) < 0;
    public static bool operator >(JmapInt left, JmapInt right) => left.CompareTo(right) > 0;
    public static bool operator <=(JmapInt left, JmapInt right) => left.CompareTo(right) <= 0;
    public static bool operator >=(JmapInt left, JmapInt right) => left.CompareTo(right) >= 0;
}