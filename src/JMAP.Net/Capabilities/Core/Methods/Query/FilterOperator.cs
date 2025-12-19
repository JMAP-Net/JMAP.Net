using System.Text.Json.Serialization;

namespace JMAP.Net.Capabilities.Core.Methods.Query;

/// <summary>
/// A filter operator that combines multiple filter conditions.
/// As per RFC 8620, Section 5.5.
/// </summary>
public class FilterOperator
{
    /// <summary>
    /// The operator type: "AND", "OR", or "NOT".
    /// </summary>
    [JsonPropertyName("operator")]
    public required FilterOperatorType Operator { get; init; }

    /// <summary>
    /// The conditions to evaluate against each record.
    /// </summary>
    [JsonPropertyName("conditions")]
    public required List<object> Conditions { get; init; }
}