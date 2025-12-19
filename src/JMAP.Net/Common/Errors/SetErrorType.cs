namespace JMAP.Net.Common.Errors;

/// <summary>
/// Standard SetError types as defined in RFC 8620, Section 5.3.
/// </summary>
public static class SetErrorType
{
    public const string Forbidden = "forbidden";
    public const string OverQuota = "overQuota";
    public const string TooLarge = "tooLarge";
    public const string RateLimit = "rateLimit";
    public const string NotFound = "notFound";
    public const string InvalidPatch = "invalidPatch";
    public const string WillDestroy = "willDestroy";
    public const string InvalidProperties = "invalidProperties";
    public const string Singleton = "singleton";
    public const string AlreadyExists = "alreadyExists";
}