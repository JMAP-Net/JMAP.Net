namespace JMAP.Net.Common.Errors;

/// <summary>
/// Standard JMAP problem detail URIs.
/// </summary>
public static class ProblemDetailsType
{
    private const string Prefix = "urn:ietf:params:jmap:error:";

    /// <summary>
    /// Problem detail URI for an unknown capability error.
    /// </summary>
    public const string UnknownCapability = Prefix + "unknownCapability";

    /// <summary>
    /// Problem detail URI for an invalid JSON payload.
    /// </summary>
    public const string NotJson = Prefix + "notJSON";

    /// <summary>
    /// Problem detail URI for a payload that is not a JMAP request.
    /// </summary>
    public const string NotRequest = Prefix + "notRequest";

    /// <summary>
    /// Problem detail URI for a request that exceeded a server limit.
    /// </summary>
    public const string Limit = Prefix + "limit";
}
