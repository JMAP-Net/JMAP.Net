using JMAP.Net.Capabilities.Calendars.Methods.Calendar;
using JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;
using JMAP.Net.Capabilities.Calendars.Methods.CalendarEventNotification;
using JMAP.Net.Capabilities.Calendars.Methods.ParticipantIdentity;
using JMAP.Net.Capabilities.Calendars.Types;
using JMAP.Net.Capabilities.Core.Methods.Query;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Common.Errors;
using JSCalendar.Net;
using JSCalendar.Net.Enums;

namespace JMAP.Net.Tests.Calendars.Infrastructure;

internal static class CalendarFixtures
{
    public static Calendar CreateCalendar()
    {
        return new Calendar
        {
            Id = new JmapId("cal1"),
            Name = "Team Calendar",
            Description = "Shared calendar",
            Color = "#00AAFF",
            SortOrder = new JmapUnsignedInt(5),
            IsSubscribed = true,
            IsVisible = true,
            IsDefault = false,
            IncludeInAvailability = CalendarAvailabilityInclusion.All,
            TimeZone = "Europe/Berlin",
            ShareWith = new Dictionary<JmapId, CalendarRights>
            {
                [new JmapId("principal1")] = new()
                {
                    MayReadFreeBusy = true,
                    MayReadItems = true,
                    MayWriteAll = false,
                    MayWriteOwn = true,
                    MayUpdatePrivate = true,
                    MayRSVP = true,
                    MayAdmin = false,
                    MayDelete = false
                }
            },
            MyRights = new CalendarRights
            {
                MayReadFreeBusy = true,
                MayReadItems = true,
                MayWriteAll = true,
                MayWriteOwn = true,
                MayUpdatePrivate = true,
                MayRSVP = true,
                MayAdmin = true,
                MayDelete = false
            }
        };
    }

    public static ParticipantIdentity CreateParticipantIdentity()
    {
        return new ParticipantIdentity
        {
            Id = new JmapId("ident1"),
            ScheduleId = "mailto:user@example.com",
            SendTo = new Dictionary<string, string>
            {
                ["imip"] = "mailto:user@example.com"
            },
            IsDefault = false
        };
    }

    public static CalendarEvent CreateCalendarEvent()
    {
        return new CalendarEvent
        {
            Id = new JmapId("event1"),
            Uid = "uid-1@example.com",
            Updated = new DateTimeOffset(2026, 4, 1, 7, 30, 0, TimeSpan.Zero),
            Start = new LocalDateTime(new DateTime(2026, 4, 1, 10, 0, 0)),
            Duration = new Duration { Hours = 1 },
            Title = "Design Review",
            TimeZone = "Europe/Berlin",
            CalendarIds = new Dictionary<JmapId, bool>
            {
                [new JmapId("cal1")] = true
            },
            IsDraft = true,
            IsOrigin = true,
            UtcStart = new JmapUtcDate(new DateTimeOffset(2026, 4, 1, 8, 0, 0, TimeSpan.Zero)),
            UtcEnd = new JmapUtcDate(new DateTimeOffset(2026, 4, 1, 9, 0, 0, TimeSpan.Zero)),
            MayInviteSelf = true,
            MayInviteOthers = true,
            HideAttendees = true,
            UseDefaultAlerts = true,
            Participants = new Dictionary<string, Participant>
            {
                ["alice"] = new JmapParticipant
                {
                    Name = "Alice",
                    Email = "alice@example.com",
                    ScheduleId = "mailto:alice@example.com",
                    Roles = new Dictionary<ParticipantRole, bool>
                    {
                        [ParticipantRole.Owner] = true
                    },
                    ParticipationStatus = ParticipationStatus.Accepted,
                    ScheduleSequence = 7,
                    ScheduleUpdated = new DateTimeOffset(2026, 4, 1, 7, 45, 0, TimeSpan.Zero)
                }
            }
        };
    }

    public static CalendarEventNotification CreateCalendarEventNotification()
    {
        return new CalendarEventNotification
        {
            Id = new JmapId("notif1"),
            Created = new JmapUtcDate(new DateTimeOffset(2026, 4, 1, 9, 30, 0, TimeSpan.Zero)),
            ChangedBy = new NotificationPerson
            {
                Name = "Alice",
                Email = "alice@example.com",
                PrincipalId = new JmapId("principal1"),
                ScheduleId = "mailto:alice@example.com"
            },
            Comment = "Updated the agenda",
            Type = CalendarEventNotificationType.Updated,
            CalendarEventId = new JmapId("event1"),
            IsDraft = false,
            Event = CreateCalendarEvent(),
            EventPatch = new PatchObject(new Dictionary<string, object?>
            {
                ["title"] = "Design Review Updated"
            })
        };
    }

    public static CalendarSetResponse CreateCalendarSetResponse()
    {
        return new CalendarSetResponse
        {
            AccountId = new JmapId("account1"),
            OldState = "state-1",
            NewState = "state-2",
            Created = new Dictionary<JmapId, Calendar>
            {
                [new JmapId("temp-cal-1")] = CreateCalendar()
            },
            Updated = new Dictionary<JmapId, Calendar?>
            {
                [new JmapId("cal2")] = null
            },
            Destroyed =
            [
                new JmapId("cal3")
            ],
            NotCreated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("temp-cal-2")] = new SetError
                {
                    Type = SetErrorType.InvalidProperties,
                    Description = "name is required",
                    Properties =
                    [
                        "name"
                    ]
                }
            },
            NotUpdated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("cal4")] = new SetError
                {
                    Type = SetErrorType.NotFound,
                    Description = "Calendar not found"
                }
            },
            NotDestroyed = new Dictionary<JmapId, SetError>
            {
                [new JmapId("cal5")] = new SetError
                {
                    Type = SetErrorType.Forbidden,
                    Description = "Calendar cannot be destroyed"
                }
            }
        };
    }

    public static CalendarQueryResponse CreateCalendarQueryResponse()
    {
        return new CalendarQueryResponse
        {
            AccountId = new JmapId("account1"),
            QueryState = "query-state-1",
            CanCalculateChanges = true,
            Position = new JmapUnsignedInt(0),
            Ids =
            [
                new JmapId("cal1"),
                new JmapId("cal2")
            ],
            Total = new JmapUnsignedInt(2),
            Limit = new JmapUnsignedInt(25)
        };
    }

    public static CalendarCopyResponse CreateCalendarCopyResponse()
    {
        return new CalendarCopyResponse
        {
            FromAccountId = new JmapId("source-account"),
            AccountId = new JmapId("target-account"),
            OldState = "state-1",
            NewState = "state-2",
            Created = new Dictionary<JmapId, Calendar>
            {
                [new JmapId("copy-cal-1")] = CreateCalendar()
            },
            NotCreated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("copy-cal-2")] = new SetError
                {
                    Type = SetErrorType.AlreadyExists,
                    Description = "A calendar with this id already exists",
                    ExistingId = "cal-existing"
                }
            }
        };
    }

    public static CalendarEventSetResponse CreateCalendarEventSetResponse()
    {
        return new CalendarEventSetResponse
        {
            AccountId = new JmapId("account1"),
            OldState = "state-1",
            NewState = "state-2",
            Created = new Dictionary<JmapId, CalendarEvent>
            {
                [new JmapId("temp-event-1")] = CreateCalendarEvent()
            },
            Updated = new Dictionary<JmapId, CalendarEvent?>
            {
                [new JmapId("event2")] = null
            },
            Destroyed =
            [
                new JmapId("event3")
            ],
            NotCreated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("temp-event-2")] = new SetError
                {
                    Type = SetErrorType.InvalidProperties,
                    Description = "calendarIds is required",
                    Properties =
                    [
                        "calendarIds"
                    ]
                }
            },
            NotUpdated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("event4")] = new SetError
                {
                    Type = SetErrorType.NotFound,
                    Description = "Calendar event not found"
                }
            },
            NotDestroyed = new Dictionary<JmapId, SetError>
            {
                [new JmapId("event5")] = new SetError
                {
                    Type = SetErrorType.Forbidden,
                    Description = "Calendar event cannot be destroyed"
                }
            }
        };
    }

    public static CalendarEventQueryResponse CreateCalendarEventQueryResponse()
    {
        return new CalendarEventQueryResponse
        {
            AccountId = new JmapId("account1"),
            QueryState = "query-state-1",
            CanCalculateChanges = true,
            Position = new JmapUnsignedInt(0),
            Ids =
            [
                new JmapId("event1"),
                new JmapId("event2")
            ],
            Total = new JmapUnsignedInt(2),
            Limit = new JmapUnsignedInt(100)
        };
    }

    public static CalendarEventCopyResponse CreateCalendarEventCopyResponse()
    {
        return new CalendarEventCopyResponse
        {
            FromAccountId = new JmapId("source-account"),
            AccountId = new JmapId("target-account"),
            OldState = "state-1",
            NewState = "state-2",
            Created = new Dictionary<JmapId, CalendarEvent>
            {
                [new JmapId("copy-event-1")] = CreateCalendarEvent()
            },
            NotCreated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("copy-event-2")] = new SetError
                {
                    Type = SetErrorType.AlreadyExists,
                    Description = "A calendar event with this id already exists",
                    ExistingId = "event-existing"
                }
            }
        };
    }

    public static ParticipantIdentitySetResponse CreateParticipantIdentitySetResponse()
    {
        return new ParticipantIdentitySetResponse
        {
            AccountId = new JmapId("account1"),
            OldState = "state-1",
            NewState = "state-2",
            Created = new Dictionary<JmapId, ParticipantIdentity>
            {
                [new JmapId("temp-ident-1")] = CreateParticipantIdentity()
            },
            Updated = new Dictionary<JmapId, ParticipantIdentity?>
            {
                [new JmapId("ident2")] = null
            },
            Destroyed =
            [
                new JmapId("ident3")
            ],
            NotCreated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("temp-ident-2")] = new SetError
                {
                    Type = SetErrorType.InvalidProperties,
                    Description = "scheduleId is required",
                    Properties =
                    [
                        "scheduleId"
                    ]
                }
            },
            NotUpdated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("ident4")] = new SetError
                {
                    Type = SetErrorType.NotFound,
                    Description = "Participant identity not found"
                }
            },
            NotDestroyed = new Dictionary<JmapId, SetError>
            {
                [new JmapId("ident5")] = new SetError
                {
                    Type = SetErrorType.Forbidden,
                    Description = "Participant identity cannot be destroyed"
                }
            }
        };
    }

    public static CalendarEventNotificationSetResponse CreateCalendarEventNotificationSetResponse()
    {
        return new CalendarEventNotificationSetResponse
        {
            AccountId = new JmapId("account1"),
            OldState = "state-1",
            NewState = "state-2",
            Created = new Dictionary<JmapId, CalendarEventNotification>
            {
                [new JmapId("temp-notif-1")] = CreateCalendarEventNotification()
            },
            Updated = new Dictionary<JmapId, CalendarEventNotification?>
            {
                [new JmapId("notif2")] = null
            },
            Destroyed =
            [
                new JmapId("notif3")
            ],
            NotCreated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("temp-notif-2")] = new SetError
                {
                    Type = SetErrorType.InvalidProperties,
                    Description = "type is required",
                    Properties =
                    [
                        "type"
                    ]
                }
            },
            NotUpdated = new Dictionary<JmapId, SetError>
            {
                [new JmapId("notif4")] = new SetError
                {
                    Type = SetErrorType.NotFound,
                    Description = "Calendar event notification not found"
                }
            },
            NotDestroyed = new Dictionary<JmapId, SetError>
            {
                [new JmapId("notif5")] = new SetError
                {
                    Type = SetErrorType.Forbidden,
                    Description = "Calendar event notification cannot be destroyed"
                }
            }
        };
    }

    public static CalendarEventNotificationQueryResponse CreateCalendarEventNotificationQueryResponse()
    {
        return new CalendarEventNotificationQueryResponse
        {
            AccountId = new JmapId("account1"),
            QueryState = "query-state-1",
            CanCalculateChanges = true,
            Position = new JmapUnsignedInt(0),
            Ids =
            [
                new JmapId("notif1"),
                new JmapId("notif2")
            ],
            Total = new JmapUnsignedInt(2),
            Limit = new JmapUnsignedInt(50)
        };
    }
}
