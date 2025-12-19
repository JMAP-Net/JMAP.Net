using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using JMAP.Net.Common.Converters;

namespace JMAP.Net.Capabilities.Core.Types;

/// <summary>
/// Represents a JMAP Id type - a string of 1-255 octets containing only URL and Filename Safe base64 characters.
/// As per RFC 8620, Section 1.2: Characters allowed are A-Za-z0-9, hyphen (-), and underscore (_).
/// </summary>
[JsonConverter(typeof(JmapIdJsonConverter))]
public readonly partial struct JmapId : IEquatable<JmapId>
{
    private const int MinLength = 1;
    private const int MaxLength = 255;

    [GeneratedRegex("^[A-Za-z0-9_-]+$")]
    private static partial Regex ValidCharactersRegex();

    public string Value { get; }

    public JmapId(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("JMAP Id cannot be null or empty", nameof(value));

        if (value.Length is < MinLength or > MaxLength)
            throw new ArgumentException(
                $"JMAP Id must be between {MinLength} and {MaxLength} octets in size",
                nameof(value));

        if (!ValidCharactersRegex().IsMatch(value))
            throw new ArgumentException(
                "JMAP Id must only contain characters from A-Za-z0-9, hyphen (-), and underscore (_)",
                nameof(value));

        Value = value;
    }

    /// <summary>
    /// Validates whether a string is a valid JMAP Id without throwing an exception.
    /// </summary>
    /// <param name="value">The string to validate</param>
    /// <returns>True if the string is a valid JMAP Id, false otherwise</returns>
    public static bool IsValid(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        if (value.Length is < MinLength or > MaxLength)
            return false;

        return ValidCharactersRegex().IsMatch(value);
    }

    /// <summary>
    /// Tries to create a JmapId from a string value.
    /// </summary>
    /// <param name="value">The string to parse</param>
    /// <param name="id">The resulting JmapId if successful</param>
    /// <returns>True if parsing was successful, false otherwise</returns>
    public static bool TryParse(string? value, out JmapId id)
    {
        if (IsValid(value))
        {
            id = new JmapId(value!);
            return true;
        }

        id = default;
        return false;
    }

    /// <summary>
    /// Checks if this ID follows JMAP best practices to avoid security issues.
    /// Per RFC 8620, Section 1.2, servers SHOULD avoid:
    /// - IDs starting with a dash
    /// - IDs starting with digits
    /// - IDs that contain only digits
    /// - IDs that differ only by ASCII case
    /// - The specific sequence "NIL"
    /// </summary>
    /// <returns>True if the ID follows best practices, false otherwise</returns>
    public bool FollowsBestPractices()
    {
        // Should not start with dash
        if (Value.StartsWith('-'))
            return false;

        // Should not start with digit
        if (char.IsDigit(Value[0]))
            return false;

        // Should not be all digits
        if (Value.All(char.IsDigit))
            return false;

        // Should not be "NIL" (case insensitive)
        if (Value.Equals("NIL", StringComparison.OrdinalIgnoreCase))
            return false;

        return true;
    }

    public static implicit operator string(JmapId id) => id.Value;
    public static explicit operator JmapId(string value) => new(value);

    public override string ToString() => Value;
    public bool Equals(JmapId other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is JmapId other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(JmapId left, JmapId right) => left.Equals(right);
    public static bool operator !=(JmapId left, JmapId right) => !left.Equals(right);
}