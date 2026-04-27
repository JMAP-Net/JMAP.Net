using System.Globalization;
using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Converters;

namespace JMAP.Net.Capabilities.Core.Types;

/// <summary>
/// Represents a JMAP UTCDate type - a Date where the time-offset MUST be Z (UTC time).
/// As per RFC 8620, Section 1.4.
/// Example: "2014-10-30T06:12:00Z"
/// </summary>
[JsonConverter(typeof(JmapUtcDateJsonConverter))]
public readonly struct JmapUtcDate : IEquatable<JmapUtcDate>, IComparable<JmapUtcDate>
{
    /// <summary>
    /// Gets the underlying UTC date-time value.
    /// </summary>
    public DateTimeOffset Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="JmapUtcDate" /> struct from a
    /// <see cref="DateTimeOffset" /> value.
    /// </summary>
    /// <param name="value">The value to convert to UTC and store.</param>
    public JmapUtcDate(DateTimeOffset value)
    {
        // Convert to UTC if not already
        Value = value.ToUniversalTime();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JmapUtcDate" /> struct from a
    /// <see cref="DateTime" /> value that is treated as UTC.
    /// </summary>
    /// <param name="value">The UTC value to store.</param>
    public JmapUtcDate(DateTime value)
    {
        // Treat as UTC
        Value = new DateTimeOffset(value, TimeSpan.Zero);
    }

    /// <summary>
    /// Converts a <see cref="JmapUtcDate" /> to its underlying <see cref="DateTimeOffset" /> value.
    /// </summary>
    public static implicit operator DateTimeOffset(JmapUtcDate date) => date.Value;

    /// <summary>
    /// Converts a <see cref="DateTimeOffset" /> to a <see cref="JmapUtcDate" />.
    /// </summary>
    public static implicit operator JmapUtcDate(DateTimeOffset value) => new(value);

    /// <summary>
    /// Converts a <see cref="DateTime" /> to a <see cref="JmapUtcDate" />.
    /// </summary>
    public static implicit operator JmapUtcDate(DateTime value) => new(value);

    /// <summary>
    /// Returns the date in normalized RFC 3339 UTC format with uppercase letters
    /// and no fractional seconds if they are zero.
    /// </summary>
    public override string ToString()
    {
        var utc = Value.ToUniversalTime();
        var formatted = utc.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFFZ", CultureInfo.InvariantCulture);
        
        // Remove trailing zeros from fractional seconds
        if (formatted.Contains('.'))
        {
            var dateTimePart = formatted.Substring(0, formatted.Length - 1); // Remove 'Z'
            dateTimePart = dateTimePart.TrimEnd('0');
            if (dateTimePart.EndsWith('.'))
                dateTimePart = dateTimePart.TrimEnd('.');
            
            formatted = dateTimePart + "Z";
        }
        
        return formatted;
    }

    /// <summary>
    /// Indicates whether this value is equal to another <see cref="JmapUtcDate" />.
    /// </summary>
    public bool Equals(JmapUtcDate other) => Value.Equals(other.Value);

    /// <summary>
    /// Indicates whether this value is equal to another object.
    /// </summary>
    public override bool Equals(object? obj) => obj is JmapUtcDate other && Equals(other);

    /// <summary>
    /// Returns a hash code for the current value.
    /// </summary>
    public override int GetHashCode() => Value.GetHashCode();

    /// <summary>
    /// Compares this value with another <see cref="JmapUtcDate" />.
    /// </summary>
    public int CompareTo(JmapUtcDate other) => Value.CompareTo(other.Value);

    /// <summary>
    /// Returns <see langword="true" /> if two values are equal.
    /// </summary>
    public static bool operator ==(JmapUtcDate left, JmapUtcDate right) => left.Equals(right);

    /// <summary>
    /// Returns <see langword="true" /> if two values are not equal.
    /// </summary>
    public static bool operator !=(JmapUtcDate left, JmapUtcDate right) => !left.Equals(right);

    /// <summary>
    /// Returns <see langword="true" /> if the left value is less than the right value.
    /// </summary>
    public static bool operator <(JmapUtcDate left, JmapUtcDate right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Returns <see langword="true" /> if the left value is greater than the right value.
    /// </summary>
    public static bool operator >(JmapUtcDate left, JmapUtcDate right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Returns <see langword="true" /> if the left value is less than or equal to the right value.
    /// </summary>
    public static bool operator <=(JmapUtcDate left, JmapUtcDate right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Returns <see langword="true" /> if the left value is greater than or equal to the right value.
    /// </summary>
    public static bool operator >=(JmapUtcDate left, JmapUtcDate right) => left.CompareTo(right) >= 0;
}
