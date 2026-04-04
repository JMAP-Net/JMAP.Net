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

    /// <summary>
    /// Gets the underlying integer value.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="JmapInt" /> struct.
    /// </summary>
    /// <param name="value">The integer value to validate and store.</param>
    public JmapInt(long value)
    {
        if (value is < MinValue or > MaxValue)
            throw new ArgumentOutOfRangeException(
                nameof(value),
                $"JMAP Int must be in the range {MinValue} to {MaxValue}");

        Value = value;
    }

    /// <summary>
    /// Converts a <see cref="JmapInt" /> to a <see cref="long" />.
    /// </summary>
    public static implicit operator long(JmapInt value) => value.Value;

    /// <summary>
    /// Converts a <see cref="long" /> to a <see cref="JmapInt" />.
    /// </summary>
    public static explicit operator JmapInt(long value) => new(value);

    /// <summary>
    /// Converts an <see cref="int" /> to a <see cref="JmapInt" />.
    /// </summary>
    public static explicit operator JmapInt(int value) => new(value);

    /// <summary>
    /// Returns the numeric value as a string.
    /// </summary>
    public override string ToString() => Value.ToString();

    /// <summary>
    /// Indicates whether this value is equal to another <see cref="JmapInt" />.
    /// </summary>
    public bool Equals(JmapInt other) => Value == other.Value;

    /// <summary>
    /// Indicates whether this value is equal to another object.
    /// </summary>
    public override bool Equals(object? obj) => obj is JmapInt other && Equals(other);

    /// <summary>
    /// Returns a hash code for the current value.
    /// </summary>
    public override int GetHashCode() => Value.GetHashCode();

    /// <summary>
    /// Compares this value with another <see cref="JmapInt" />.
    /// </summary>
    public int CompareTo(JmapInt other) => Value.CompareTo(other.Value);

    /// <summary>
    /// Returns <see langword="true" /> if two values are equal.
    /// </summary>
    public static bool operator ==(JmapInt left, JmapInt right) => left.Equals(right);

    /// <summary>
    /// Returns <see langword="true" /> if two values are not equal.
    /// </summary>
    public static bool operator !=(JmapInt left, JmapInt right) => !left.Equals(right);

    /// <summary>
    /// Returns <see langword="true" /> if the left value is less than the right value.
    /// </summary>
    public static bool operator <(JmapInt left, JmapInt right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Returns <see langword="true" /> if the left value is greater than the right value.
    /// </summary>
    public static bool operator >(JmapInt left, JmapInt right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Returns <see langword="true" /> if the left value is less than or equal to the right value.
    /// </summary>
    public static bool operator <=(JmapInt left, JmapInt right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Returns <see langword="true" /> if the left value is greater than or equal to the right value.
    /// </summary>
    public static bool operator >=(JmapInt left, JmapInt right) => left.CompareTo(right) >= 0;
}
