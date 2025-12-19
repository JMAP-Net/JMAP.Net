using System.Text.Json.Serialization;

namespace JMAP.Net.Capabilities.Core.Methods.Query;

/// <summary>
/// Filter operator types.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<FilterOperatorType>))]
public enum FilterOperatorType
{
    /// <summary>
    /// All of the conditions must match for the filter to match.
    /// </summary>
    AND,

    /// <summary>
    /// At least one of the conditions must match for the filter to match.
    /// </summary>
    OR,

    /// <summary>
    /// None of the conditions must match for the filter to match.
    /// </summary>
    NOT
}