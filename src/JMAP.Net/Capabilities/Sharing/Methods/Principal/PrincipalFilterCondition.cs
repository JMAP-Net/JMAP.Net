using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Capabilities.Sharing.Types;

namespace JMAP.Net.Capabilities.Sharing.Methods.Principal;

/// <summary>
/// A filter condition for Principal/query.
/// </summary>
public sealed class PrincipalFilterCondition
{
    /// <summary>
    /// A list of account ids; the Principal matches if any id is present in the Principal accounts map.
    /// </summary>
    [JsonPropertyName("accountIds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<JmapId>? AccountIds { get; init; }

    /// <summary>
    /// The Principal email contains this string.
    /// </summary>
    [JsonPropertyName("email")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Email { get; init; }

    /// <summary>
    /// The Principal name contains this string.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; init; }

    /// <summary>
    /// The Principal name, email, or description contains this string.
    /// </summary>
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; init; }

    /// <summary>
    /// The Principal type must exactly match this value.
    /// </summary>
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PrincipalType? Type { get; init; }

    /// <summary>
    /// The Principal time zone must exactly match this value.
    /// </summary>
    [JsonPropertyName("timeZone")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TimeZone { get; init; }

    /// <summary>
    /// The given calendar address belongs to the Principal.
    /// This filter is defined by the JMAP Calendars availability extension.
    /// </summary>
    [JsonPropertyName("calendarAddress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CalendarAddress { get; init; }
}
