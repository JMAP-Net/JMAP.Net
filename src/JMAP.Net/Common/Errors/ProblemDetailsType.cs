namespace JMAP.Net.Common.Errors;

/// <summary>
/// Standard JMAP problem detail URIs.
/// </summary>
public static class ProblemDetailsType
{
    private const string Prefix = "urn:ietf:params:jmap:error:";

    public const string UnknownCapability = Prefix + "unknownCapability";
    public const string NotJson = Prefix + "notJSON";
    public const string NotRequest = Prefix + "notRequest";
    public const string Limit = Prefix + "limit";
}