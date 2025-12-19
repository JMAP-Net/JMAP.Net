# JMAP.Net

[![.NET](https://img.shields.io/badge/.NET-10.0-purple)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

A modern .NET implementation of [RFC 8620 (JMAP Core)](https://jmap.io/spec-core.html) - the JSON Meta Application Protocol.

## Features

- **Full RFC 8620 Compliance** - Complete implementation of the JMAP Core specification
- **Native .NET 10** - Built with the latest .NET features
- **Type-Safe** - Strongly-typed models with comprehensive validation
- **JSON Serialization** - Seamless integration with System.Text.Json
- **Standard Methods** - Support for /get, /set, /changes, /query, /queryChanges, /copy
- **Extensible** - Designed to work with JMAP extensions (Mail, Calendars, Contacts)

## Installation

```bash
dotnet add package JMAP.Net
```

Or via NuGet Package Manager:

```
Install-Package JMAP.Net
```

## Quick Start

### Creating a JMAP Session

```csharp
using JMAP.Net.Capabilities.Core;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Common.Session;
using System.Text.Json;

var session = new JmapSession
{
    Capabilities = new Dictionary<string, object>
    {
        [CoreCapability.CapabilityUri] = new CoreCapability
        {
            MaxSizeUpload = new JmapUnsignedInt(50000000),
            MaxConcurrentUpload = new JmapUnsignedInt(4),
            MaxSizeRequest = new JmapUnsignedInt(10000000),
            MaxConcurrentRequests = new JmapUnsignedInt(4),
            MaxCallsInRequest = new JmapUnsignedInt(16),
            MaxObjectsInGet = new JmapUnsignedInt(500),
            MaxObjectsInSet = new JmapUnsignedInt(500),
            CollationAlgorithms = new List<string> { "i;ascii-casemap" }
        }
    },
    Accounts = new Dictionary<JmapId, JmapAccount>
    {
        [new JmapId("account1")] = new JmapAccount
        {
            Name = "user@example.com",
            IsPersonal = true,
            IsReadOnly = false,
            AccountCapabilities = new Dictionary<string, object>()
        }
    },
    PrimaryAccounts = new Dictionary<string, JmapId>(),
    Username = "user@example.com",
    ApiUrl = "https://jmap.example.com/api/",
    DownloadUrl = "https://jmap.example.com/download/{accountId}/{blobId}/{name}?type={type}",
    UploadUrl = "https://jmap.example.com/upload/{accountId}/",
    EventSourceUrl = "https://jmap.example.com/eventsource/?types={types}&closeafter={closeafter}&ping={ping}",
    State = "initial-state"
};

// Serialize to JSON
var json = JsonSerializer.Serialize(session, new JsonSerializerOptions 
{ 
    WriteIndented = true 
});
```

### Creating a JMAP Request

```csharp
using JMAP.Net.Common.Protocol;
using JMAP.Net.Capabilities.Core.Methods;

var request = new JmapRequest
{
    Using = new List<string> 
    { 
        CoreCapability.CapabilityUri 
    },
    MethodCalls = new List<Invocation>
    {
        new Invocation
        {
            Name = "Core/echo",
            Arguments = new Dictionary<string, object?>
            {
                ["hello"] = "world",
                ["test"] = 123
            },
            MethodCallId = "call1"
        }
    },
    CreatedIds = null
};

var requestJson = JsonSerializer.Serialize(request, new JsonSerializerOptions 
{ 
    WriteIndented = true 
});
```

**Output:**

```json
{
  "using": [
    "urn:ietf:params:jmap:core"
  ],
  "methodCalls": [
    [
      "Core/echo",
      {
        "hello": "world",
        "test": 123
      },
      "call1"
    ]
  ]
}
```

### Working with Standard Methods

#### Foo/get Request

```csharp
using JMAP.Net.Capabilities.Core.Methods;

var getRequest = new GetRequest
{
    AccountId = new JmapId("account1"),
    Ids = new List<JmapId> 
    { 
        new JmapId("id1"), 
        new JmapId("id2") 
    },
    Properties = new List<string> { "name", "description" }
};
```

#### Foo/set Request

```csharp
var setRequest = new SetRequest
{
    AccountId = new JmapId("account1"),
    IfInState = "current-state",
    Create = new Dictionary<JmapId, object>
    {
        [new JmapId("temp1")] = new 
        { 
            name = "New Item",
            description = "A new item to create"
        }
    },
    Update = new Dictionary<JmapId, Dictionary<string, object?>>
    {
        [new JmapId("existing1")] = new Dictionary<string, object?>
        {
            ["name"] = "Updated Name"
        }
    },
    Destroy = new List<JmapId> { new JmapId("old1") }
};
```

#### Foo/query Request

```csharp
using JMAP.Net.Capabilities.Core.Methods.Query;

var queryRequest = new QueryRequest
{
    AccountId = new JmapId("account1"),
    Filter = null, // or FilterOperator/FilterCondition
    Sort = new List<Comparator>
    {
        new Comparator
        {
            Property = "name",
            IsAscending = true
        }
    },
    Position = new JmapInt(0),
    Limit = new JmapUnsignedInt(10),
    CalculateTotal = true
};
```

## Core Data Types

### JmapId

Represents a JMAP Id - a string of 1-255 characters containing only URL-safe base64 characters (A-Za-z0-9, -, _).

```csharp
var id = new JmapId("account-123");
string idValue = id.Value; // "account-123"
```

### JmapInt and JmapUnsignedInt

Safe integer types that fit within JavaScript's Number type range.

```csharp
var signedInt = new JmapInt(-12345);
var unsignedInt = new JmapUnsignedInt(67890);
```

### JmapDate and JmapUtcDate

RFC 3339 date-time formats.

```csharp
var date = new JmapDate(DateTimeOffset.Now);
var utcDate = new JmapUtcDate(DateTime.UtcNow);

Console.WriteLine(date.ToString());    // "2024-12-03T14:30:00+01:00"
Console.WriteLine(utcDate.ToString()); // "2024-12-03T13:30:00Z"
```

## Error Handling

JMAP.Net provides comprehensive error handling at multiple levels:

### Request-Level Errors (RFC 7807 Problem Details)

```csharp
using JMAP.Net.Common.Errors;

var problemDetails = new ProblemDetails
{
    Type = ProblemDetailsType.UnknownCapability,
    Status = 400,
    Detail = "The capability 'urn:example:unsupported' is not supported by this server."
};
```

### Method-Level Errors

```csharp
var error = new JmapError
{
    Type = JmapErrorType.InvalidArguments,
    Description = "The 'ids' parameter must not be null"
};
```

### SetError for /set Operations

```csharp
var setError = new SetError
{
    Type = SetErrorType.NotFound,
    Description = "The object with id 'xyz' does not exist"
};
```

## Architecture

JMAP.Net follows a modular architecture with clear separation between Core protocol and Extensions:

### Core Protocol (RFC 8620)
- **JMAP.Net.Capabilities.Core** - Core data types (JmapId, JmapInt, JmapDate, CoreCapability)
- **JMAP.Net.Common.Session** - Session and account management
- **JMAP.Net.Common.Protocol** - Request/Response/Invocation structures
- **JMAP.Net.Capabilities.Core.Methods** - Standard JMAP methods (/get, /set, /changes, /query, /copy)
- **JMAP.Net.Common.Errors** - Error types and problem details
- **JMAP.Net.Common.Converters** - JSON converters for custom types

### Extensions
- **JMAP.Net.Capabilities.Calendars** - JMAP Calendars implementation (RFC 8984, integrates with JSCalendar.Net)
- **JMAP.Net.Capabilities.Contacts** - (Planned) JMAP Contacts

## JMAP Calendar Extensions

JMAP.Net includes full support for JMAP Calendars (RFC 8984) with extensions to JSCalendar for JMAP-specific features.

### Calendar Event Sharing Properties

CalendarEvent includes JMAP-specific properties for controlling event sharing and participant permissions:

```csharp
using JMAP.Net.Capabilities.Calendars;
using JMAP.Net.Capabilities.Calendars.Types;
using JMAP.Net.Capabilities.Core;
using JMAP.Net.Capabilities.Core.Types;

// Allow attendees to invite themselves
var publicEvent = new CalendarEvent
{
    Id = new JmapId("event-123"),
    Title = "Public Meetup",
    MayInviteSelf = true,
    MayInviteOthers = true
};

// Private event with hidden attendee list
var privateEvent = new CalendarEvent
{
    Id = new JmapId("event-456"),
    Title = "Private Meeting",
    HideAttendees = true
};
```

### Using JMAP Participants with scheduleId

When working with JMAP Calendars and scheduling, use `JmapParticipant` which extends JSCalendar's Participant with the `scheduleId` property:

```csharp
using JMAP.Net.Capabilities.Calendars;
using JMAP.Net.Capabilities.Calendars.Types;

var evt = new CalendarEvent
{
    Title = "Team Standup",
    Participants = new Dictionary<string, JSCalendar.Net.Participant>
    {
        ["alice"] = new JmapParticipant
        {
            Name = "Alice Smith",
            Email = "alice@example.com",
            ScheduleId = "mailto:alice@example.com", // JMAP-specific property
            Roles = new Dictionary<JSCalendar.Net.Enums.ParticipantRole, bool>
            {
                [JSCalendar.Net.Enums.ParticipantRole.Owner] = true,
                [JSCalendar.Net.Enums.ParticipantRole.Chair] = true
            },
            ParticipationStatus = JSCalendar.Net.Enums.ParticipationStatus.Accepted
        },
        ["bob"] = new JmapParticipant
        {
            Name = "Bob Johnson",
            Email = "bob@example.com",
            ScheduleId = "mailto:bob@example.com",
            Roles = new Dictionary<JSCalendar.Net.Enums.ParticipantRole, bool>
            {
                [JSCalendar.Net.Enums.ParticipantRole.Attendee] = true
            },
            ParticipationStatus = JSCalendar.Net.Enums.ParticipationStatus.Tentative,
            ExpectReply = true
        }
    }
};
```

### Working with Calendar Event Notifications

CalendarEventNotification objects track changes made by other users to shared calendar events:

```csharp
using JMAP.Net.Capabilities.Calendars;
using JMAP.Net.Capabilities.Calendars.Types;
using JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;

// Query recent notifications
var queryRequest = new CalendarEventNotificationQueryRequest
{
    AccountId = new JmapId("account1"),
    Filter = new CalendarEventNotificationFilterCondition
    {
        After = new JmapUtcDate(DateTime.UtcNow.AddDays(-7)),
        Type = CalendarEventNotificationType.Updated
    },
    Sort = new List<Comparator>
    {
        new Comparator { Property = "created", IsAscending = false }
    },
    Limit = new JmapUnsignedInt(50)
};

// Get notification details
var getRequest = new CalendarEventNotificationGetRequest
{
    AccountId = new JmapId("account1"),
    Ids = new List<JmapId> { new JmapId("notification-123") }
};

// Dismiss (delete) a notification
var setRequest = new CalendarEventNotificationSetRequest
{
    AccountId = new JmapId("account1"),
    Destroy = new List<JmapId> { new JmapId("notification-123") }
};
```

### Calendar Event Notification Types

Notifications track three types of changes:

```csharp
// Event was created
var created = CalendarEventNotificationType.Created;

// Event was updated
var updated = CalendarEventNotificationType.Updated;

// Event was destroyed
var destroyed = CalendarEventNotificationType.Destroyed;
```

Each notification includes:
- `ChangedBy` - Person who made the change (with name, email, scheduleId)
- `Event` - The event data before/after the change
- `EventPatch` - For updates, a PatchObject showing the changes
- `Comment` - Optional comment from the person who made the change

## Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## RFC 8620 Compliance

This library implements the complete JMAP Core specification as defined in [RFC 8620](https://jmap.io/spec-core.html).

**Implemented Sections:**

- Section 1: Introduction and Data Types
- Section 2: The JMAP Session Resource
- Section 3: Structured Data Exchange (Request/Response)
- Section 4: The Core/echo Method
- Section 5: Standard Methods (/get, /set, /changes, /query, /queryChanges, /copy)
- Section 6: Binary Data (Partial - structures only)
- Section 7: Push Notifications (Planned)
- Section 8: Security Considerations

## Resources

- [JMAP Website](https://jmap.io/)
- [RFC 8620 - JMAP Core](https://jmap.io/spec-core.html)
- [JMAP Calendars](https://jmap.io/spec-calendars.html)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Built with ❤️ for the .NET community
- Made with ☕ and .NET
