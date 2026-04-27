namespace JMAP.Net.Common.Errors;

/// <summary>
/// Standard SetError types as defined in RFC 8620, Section 5.3.
/// </summary>
public static class SetErrorType
{
    /// <summary>
    /// The operation is not permitted for the authenticated user.
    /// </summary>
    public const string Forbidden = "forbidden";

    /// <summary>
    /// The operation would exceed the account's quota.
    /// </summary>
    public const string OverQuota = "overQuota";

    /// <summary>
    /// The supplied object or payload is too large.
    /// </summary>
    public const string TooLarge = "tooLarge";

    /// <summary>
    /// The client has exceeded a rate limit.
    /// </summary>
    public const string RateLimit = "rateLimit";

    /// <summary>
    /// The referenced record does not exist.
    /// </summary>
    public const string NotFound = "notFound";

    /// <summary>
    /// The supplied patch object is invalid.
    /// </summary>
    public const string InvalidPatch = "invalidPatch";

    /// <summary>
    /// The operation would implicitly destroy another record.
    /// </summary>
    public const string WillDestroy = "willDestroy";

    /// <summary>
    /// One or more supplied properties are invalid.
    /// </summary>
    public const string InvalidProperties = "invalidProperties";

    /// <summary>
    /// The record is a singleton and cannot be created more than once.
    /// </summary>
    public const string Singleton = "singleton";

    /// <summary>
    /// A conflicting record already exists.
    /// </summary>
    public const string AlreadyExists = "alreadyExists";

    /// <summary>
    /// The Calendar has at least one CalendarEvent assigned to it.
    /// </summary>
    public const string CalendarHasEvent = "calendarHasEvent";
}
