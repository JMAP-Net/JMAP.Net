namespace JMAP.Net.Common.Errors;

/// <summary>
/// Standard JMAP error type strings as defined in RFC 8620.
/// </summary>
public static class JmapErrorType
{
    // Request-level errors (Section 3.6.1)
    /// <summary>
    /// The request referenced a capability the server does not support.
    /// </summary>
    public const string UnknownCapability = "unknownCapability";

    /// <summary>
    /// The request body was not valid JSON.
    /// </summary>
    public const string NotJson = "notJSON";

    /// <summary>
    /// The JSON payload was valid but not a JMAP request object.
    /// </summary>
    public const string NotRequest = "notRequest";

    /// <summary>
    /// A request-level limit was exceeded.
    /// </summary>
    public const string Limit = "limit";

    // Method-level errors (Section 3.6.2)
    /// <summary>
    /// The server is temporarily unavailable.
    /// </summary>
    public const string ServerUnavailable = "serverUnavailable";

    /// <summary>
    /// The server failed while processing the method call.
    /// </summary>
    public const string ServerFail = "serverFail";

    /// <summary>
    /// The server partially completed the method call before failing.
    /// </summary>
    public const string ServerPartialFail = "serverPartialFail";

    /// <summary>
    /// The requested method name is unknown.
    /// </summary>
    public const string UnknownMethod = "unknownMethod";

    /// <summary>
    /// One or more supplied method arguments are invalid.
    /// </summary>
    public const string InvalidArguments = "invalidArguments";

    /// <summary>
    /// A result reference could not be resolved.
    /// </summary>
    public const string InvalidResultReference = "invalidResultReference";

    /// <summary>
    /// The requested operation is not permitted.
    /// </summary>
    public const string Forbidden = "forbidden";

    /// <summary>
    /// The referenced account does not exist.
    /// </summary>
    public const string AccountNotFound = "accountNotFound";

    /// <summary>
    /// The referenced account does not support the requested method.
    /// </summary>
    public const string AccountNotSupportedByMethod = "accountNotSupportedByMethod";

    /// <summary>
    /// The referenced account is read-only for the requested method.
    /// </summary>
    public const string AccountReadOnly = "accountReadOnly";

    // /get method errors
    /// <summary>
    /// The request asked for too many records in a single /get call.
    /// </summary>
    public const string RequestTooLarge = "requestTooLarge";

    // /changes method errors
    /// <summary>
    /// The server cannot calculate changes from the supplied state.
    /// </summary>
    public const string CannotCalculateChanges = "cannotCalculateChanges";

    // /query method errors
    /// <summary>
    /// The supplied anchor id could not be found in the query results.
    /// </summary>
    public const string AnchorNotFound = "anchorNotFound";

    /// <summary>
    /// The requested sort is not supported.
    /// </summary>
    public const string UnsupportedSort = "unsupportedSort";

    /// <summary>
    /// The requested filter is not supported.
    /// </summary>
    public const string UnsupportedFilter = "unsupportedFilter";

    // /queryChanges method errors
    /// <summary>
    /// The server found more changes than it can return for the request.
    /// </summary>
    public const string TooManyChanges = "tooManyChanges";

    // /copy method errors
    /// <summary>
    /// The source account does not exist.
    /// </summary>
    public const string FromAccountNotFound = "fromAccountNotFound";

    /// <summary>
    /// The source account does not support the requested copy method.
    /// </summary>
    public const string FromAccountNotSupportedByMethod = "fromAccountNotSupportedByMethod";

    // JMAP Calendars method errors
    /// <summary>
    /// The account has no participant identities with a scheduling method the server supports.
    /// </summary>
    public const string NoSupportedScheduleMethods = "noSupportedScheduleMethods";

    /// <summary>
    /// The requested recurrence expansion duration is larger than the server allows.
    /// </summary>
    public const string ExpandDurationTooLarge = "expandDurationTooLarge";

    /// <summary>
    /// The server cannot calculate occurrences for the supplied recurrence data.
    /// </summary>
    public const string CannotCalculateOccurrences = "cannotCalculateOccurrences";
}
