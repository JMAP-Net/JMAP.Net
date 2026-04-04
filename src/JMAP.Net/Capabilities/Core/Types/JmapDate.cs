using System.Globalization;
using System.Text.Json.Serialization;
using JMAP.Net.Common.Converters;

namespace JMAP.Net.Capabilities.Core.Types;

/// <summary>
/// Represents a JMAP Date type - a string in RFC 3339 date-time format.
/// The time-secfrac MUST be omitted if zero, and letters MUST be uppercase.
/// As per RFC 8620, Section 1.4.
/// Example: "2014-10-30T14:12:00+08:00"
/// </summary>
[JsonConverter(typeof(JmapDateJsonConverter))]
public readonly struct JmapDate(DateTimeOffset value) : IEquatable<JmapDate>, IComparable<JmapDate>
{
    /// <summary>
    /// Gets the underlying date-time value.
    /// </summary>
    public DateTimeOffset Value { get; } = value;

    /// <summary>
    /// Converts a <see cref="JmapDate" /> to its underlying <see cref="DateTimeOffset" /> value.
    /// </summary>
    public static implicit operator DateTimeOffset(JmapDate date) => date.Value;

    /// <summary>
    /// Converts a <see cref="DateTimeOffset" /> to a <see cref="JmapDate" />.
    /// </summary>
    public static implicit operator JmapDate(DateTimeOffset value) => new(value);

    /// <summary>
    /// Returns the date in normalized RFC 3339 format with uppercase letters
    /// and no fractional seconds if they are zero.
    /// </summary>
    public override string ToString()
    {
        var formatted = Value.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFFzzz", CultureInfo.InvariantCulture);

        // Remove trailing zeros from fractional seconds
        if (formatted.Contains('.'))
        {
            var parts = formatted.Split('+', '-');
            var dateTimePart = parts[0];
            var offsetPart = formatted[dateTimePart.Length..];

            dateTimePart = dateTimePart.TrimEnd('0');
            if (dateTimePart.EndsWith('.'))
                dateTimePart = dateTimePart.TrimEnd('.');

            formatted = dateTimePart + offsetPart;
        }

        // Ensure uppercase
        return formatted.ToUpperInvariant();
    }

    /// <summary>
    /// Indicates whether this value is equal to another <see cref="JmapDate" />.
    /// </summary>
    public bool Equals(JmapDate other) => Value.Equals(other.Value);

    /// <summary>
    /// Indicates whether this value is equal to another object.
    /// </summary>
    public override bool Equals(object? obj) => obj is JmapDate other && Equals(other);

    /// <summary>
    /// Returns a hash code for the current value.
    /// </summary>
    public override int GetHashCode() => Value.GetHashCode();

    /// <summary>
    /// Compares this value with another <see cref="JmapDate" />.
    /// </summary>
    public int CompareTo(JmapDate other) => Value.CompareTo(other.Value);

    /// <summary>
    /// Returns <see langword="true" /> if two values are equal.
    /// </summary>
    public static bool operator ==(JmapDate left, JmapDate right) => left.Equals(right);

    /// <summary>
    /// Returns <see langword="true" /> if two values are not equal.
    /// </summary>
    public static bool operator !=(JmapDate left, JmapDate right) => !left.Equals(right);

    /// <summary>
    /// Returns <see langword="true" /> if the left value is less than the right value.
    /// </summary>
    public static bool operator <(JmapDate left, JmapDate right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Returns <see langword="true" /> if the left value is greater than the right value.
    /// </summary>
    public static bool operator >(JmapDate left, JmapDate right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Returns <see langword="true" /> if the left value is less than or equal to the right value.
    /// </summary>
    public static bool operator <=(JmapDate left, JmapDate right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Returns <see langword="true" /> if the left value is greater than or equal to the right value.
    /// </summary>
    public static bool operator >=(JmapDate left, JmapDate right) => left.CompareTo(right) >= 0;
}
