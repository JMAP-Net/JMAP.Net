using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;
using JSCalendar.Net;

namespace JMAP.Net.Capabilities.Calendars.Types;

/// <summary>
/// A CalendarEventNotification records changes made by external entities 
/// to events in calendars the user is subscribed to.
/// Notifications are stored in the same Account as the CalendarEvent that was changed.
/// As per RFC 8984, Section 6.
/// </summary>
public class CalendarEventNotification
{
    /// <summary>
    /// The id of the CalendarEventNotification.
    /// Server-set and immutable.
    /// </summary>
    [JsonPropertyName("id")]
    public required JmapId Id { get; init; }

    /// <summary>
    /// The time this notification was created.
    /// Server-set.
    /// </summary>
    [JsonPropertyName("created")]
    public required JmapUtcDate Created { get; init; }

    /// <summary>
    /// Who made the change.
    /// </summary>
    [JsonPropertyName("changedBy")]
    public required NotificationPerson ChangedBy { get; init; }

    /// <summary>
    /// Comment sent along with the change by the user that made it
    /// (e.g. COMMENT property in an iTIP message), if any.
    /// </summary>
    [JsonPropertyName("comment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Comment { get; init; }

    /// <summary>
    /// The type of change. 
    /// MUST be one of: "created", "updated", "destroyed".
    /// </summary>
    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required CalendarEventNotificationType Type { get; init; }

    /// <summary>
    /// The id of the CalendarEvent that this notification is about.
    /// Note: For a recurring event this is the id of the base event, 
    /// never a synthetic id for a particular instance.
    /// </summary>
    [JsonPropertyName("calendarEventId")]
    public required JmapId CalendarEventId { get; init; }

    /// <summary>
    /// Is this event a draft? (for created/updated only)
    /// </summary>
    [JsonPropertyName("isDraft")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsDraft { get; init; }

    /// <summary>
    /// The event data before the change (if updated or destroyed), 
    /// or the data after creation (if created).
    /// If the change only affects a single instance of a recurring event,
    /// the server MAY set this to just that instance.
    /// </summary>
    [JsonPropertyName("event")]
    public required Event Event { get; init; }

    /// <summary>
    /// A patch encoding the change between the data in the event property,
    /// and the data after the update.
    /// Only present when Type is "updated".
    /// </summary>
    [JsonPropertyName("eventPatch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PatchObject? EventPatch { get; init; }
}

/// <summary>
/// Notification type enumeration.
/// </summary>
public enum CalendarEventNotificationType
{
    /// <summary>Event was created</summary>
    [JsonPropertyName("created")]
    Created,

    /// <summary>Event was updated</summary>
    [JsonPropertyName("updated")]
    Updated,

    /// <summary>Event was destroyed</summary>
    [JsonPropertyName("destroyed")]
    Destroyed
}

/// <summary>
/// Information about the person who made a change to a calendar event.
/// </summary>
public class NotificationPerson
{
    /// <summary>
    /// The name of the person who made the change.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// The email of the person who made the change, or null if no email is available.
    /// </summary>
    [JsonPropertyName("email")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Email { get; init; }

    /// <summary>
    /// The id of the Principal corresponding to the person who made the change, if any.
    /// This will be null if the change was due to receiving an iTIP message.
    /// </summary>
    [JsonPropertyName("principalId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapId? PrincipalId { get; init; }

    /// <summary>
    /// The scheduleId URI of the person who made the change, if any.
    /// This will normally be set if the change was made due to receiving an iTIP message.
    /// </summary>
    [JsonPropertyName("scheduleId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ScheduleId { get; init; }
}
