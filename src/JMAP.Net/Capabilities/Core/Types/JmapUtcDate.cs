using System.Globalization;
using System.Text.Json.Serialization;
using JMAP.Net.Common.Converters;

namespace JMAP.Net.Capabilities.Core.Types;

/// <summary>
/// Represents a JMAP UTCDate type - a Date where the time-offset MUST be Z (UTC time).
/// As per RFC 8620, Section 1.4.
/// Example: "2014-10-30T06:12:00Z"
/// </summary>
[JsonConverter(typeof(JmapUtcDateJsonConverter))]
public readonly struct JmapUtcDate : IEquatable<JmapUtcDate>, IComparable<JmapUtcDate>
{
    public DateTimeOffset Value { get; }

    public JmapUtcDate(DateTimeOffset value)
    {
        // Convert to UTC if not already
        Value = value.ToUniversalTime();
    }

    public JmapUtcDate(DateTime value)
    {
        // Treat as UTC
        Value = new DateTimeOffset(value, TimeSpan.Zero);
    }

    public static implicit operator DateTimeOffset(JmapUtcDate date) => date.Value;
    public static implicit operator JmapUtcDate(DateTimeOffset value) => new(value);
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

    public bool Equals(JmapUtcDate other) => Value.Equals(other.Value);
    public override bool Equals(object? obj) => obj is JmapUtcDate other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    
    public int CompareTo(JmapUtcDate other) => Value.CompareTo(other.Value);
    
    public static bool operator ==(JmapUtcDate left, JmapUtcDate right) => left.Equals(right);
    public static bool operator !=(JmapUtcDate left, JmapUtcDate right) => !left.Equals(right);
    public static bool operator <(JmapUtcDate left, JmapUtcDate right) => left.CompareTo(right) < 0;
    public static bool operator >(JmapUtcDate left, JmapUtcDate right) => left.CompareTo(right) > 0;
    public static bool operator <=(JmapUtcDate left, JmapUtcDate right) => left.CompareTo(right) <= 0;
    public static bool operator >=(JmapUtcDate left, JmapUtcDate right) => left.CompareTo(right) >= 0;
}