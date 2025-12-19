namespace JMAP.Net.Common.Errors;

/// <summary>
/// Standard JMAP error types as defined in RFC 8620.
/// </summary>
public static class JmapErrorType
{
    // Request-level errors (Section 3.6.1)
    public const string UnknownCapability = "unknownCapability";
    public const string NotJson = "notJSON";
    public const string NotRequest = "notRequest";
    public const string Limit = "limit";

    // Method-level errors (Section 3.6.2)
    public const string ServerUnavailable = "serverUnavailable";
    public const string ServerFail = "serverFail";
    public const string ServerPartialFail = "serverPartialFail";
    public const string UnknownMethod = "unknownMethod";
    public const string InvalidArguments = "invalidArguments";
    public const string InvalidResultReference = "invalidResultReference";
    public const string Forbidden = "forbidden";
    public const string AccountNotFound = "accountNotFound";
    public const string AccountNotSupportedByMethod = "accountNotSupportedByMethod";
    public const string AccountReadOnly = "accountReadOnly";

    // /get method errors
    public const string RequestTooLarge = "requestTooLarge";

    // /changes method errors
    public const string CannotCalculateChanges = "cannotCalculateChanges";

    // /query method errors
    public const string AnchorNotFound = "anchorNotFound";
    public const string UnsupportedSort = "unsupportedSort";
    public const string UnsupportedFilter = "unsupportedFilter";

    // /queryChanges method errors
    public const string TooManyChanges = "tooManyChanges";

    // /copy method errors
    public const string FromAccountNotFound = "fromAccountNotFound";
    public const string FromAccountNotSupportedByMethod = "fromAccountNotSupportedByMethod";
}