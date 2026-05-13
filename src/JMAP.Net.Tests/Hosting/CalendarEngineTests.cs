using System.Security.Claims;
using System.Text.Json;
using JMAP.Net.Capabilities.Calendars.Methods.Calendar;
using JMAP.Net.Capabilities.Calendars.Types;
using JMAP.Net.Capabilities.Core.Methods.Query;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Common.Errors;
using JMAP.Net.Hosting;
using JMAP.Net.Hosting.Engine;
using JMAP.Net.Hosting.Engine.Calendar;
using JMAP.Net.Hosting.Services;
using JMAP.Net.Persistence.Calendars;
using JMAP.Net.Persistence.Stores;
using JMAP.Net.Tests.Hosting.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace JMAP.Net.Tests.Hosting;

public class CalendarEngineTests : JmapHostingTestBase
{
    [Test]
    public async Task CalendarEngine_GetAsync_WhenIdsAreSpecified_ShouldReturnListAndNotFound()
    {
        var engine = new CalendarEngine(new InMemoryCalendarStore());

        var response = await engine.GetAsync(
            CreateExecutionContext(new HashSet<JmapId> { new("account1") }),
            new CalendarGetRequest
            {
                AccountId = new JmapId("account1"),
                Ids =
                [
                    new JmapId("calendar1"),
                    new JmapId("missing-calendar")
                ]
            });

        await Assert.That(response.State).IsEqualTo("calendar-state-1");
        using var _ = Assert.Multiple();
        await Assert.That(response.List.Count).IsEqualTo(1);
        await Assert.That(response.List[0].Id).IsEqualTo(new JmapId("calendar1"));
        await Assert.That(response.NotFound.Count).IsEqualTo(1);
        await Assert.That(response.NotFound[0]).IsEqualTo(new JmapId("missing-calendar"));
    }

    [Test]
    public async Task CalendarEngine_QueryAsync_ShouldReturnQueryShape()
    {
        var engine = new CalendarEngine(new InMemoryCalendarStore());

        var response = await engine.QueryAsync(
            CreateExecutionContext(new HashSet<JmapId> { new("account1") }),
            new CalendarQueryRequest
            {
                AccountId = new JmapId("account1"),
                CalculateTotal = true
            });

        using var _ = Assert.Multiple();
        await Assert.That(response.QueryState).IsEqualTo("calendar-state-1");
        await Assert.That(response.CanCalculateChanges).IsTrue();
        await Assert.That(response.Position).IsEqualTo(new JmapUnsignedInt(0));
        await Assert.That(response.Ids[0]).IsEqualTo(new JmapId("calendar1"));
        await Assert.That(response.Total).IsEqualTo(new JmapUnsignedInt(1));
    }

    [Test]
    public async Task CalendarEngine_ChangesAsync_ShouldReturnChangesShape()
    {
        var engine = new CalendarEngine(new InMemoryCalendarStore());

        var response = await engine.ChangesAsync(
            CreateExecutionContext(new HashSet<JmapId> { new("account1") }),
            new CalendarChangesRequest
            {
                AccountId = new JmapId("account1"),
                SinceState = "calendar-state-0"
            });

        using var _ = Assert.Multiple();
        await Assert.That(response.OldState).IsEqualTo("calendar-state-0");
        await Assert.That(response.NewState).IsEqualTo("calendar-state-1");
        await Assert.That(response.HasMoreChanges).IsFalse();
        await Assert.That(response.Created[0]).IsEqualTo(new JmapId("calendar1"));
    }

    [Test]
    public async Task CalendarEngine_GetAsync_WhenAccountIsNotAllowed_ShouldThrowAccountNotFound()
    {
        var engine = new CalendarEngine(new InMemoryCalendarStore());

        var act = async () => await engine.GetAsync(
            CreateExecutionContext(new HashSet<JmapId> { new("other-account") }),
            new CalendarGetRequest
            {
                AccountId = new JmapId("account1")
            });

        var exception = await Assert.ThrowsAsync<JmapMethodException>(act);
        await Assert.That(exception!.ErrorType).IsEqualTo("accountNotFound");
    }

    [Test]
    public async Task Dispatcher_WhenCalendarGetHasProperties_ShouldReturnProjectedCalendar()
    {
        await using var scope = CreateServiceScope(server =>
            server.AddCalendarEngine<InMemoryCalendarStore, TestUserContextProvider>());
        var dispatcher = scope.ServiceProvider.GetRequiredService<IJmapRequestDispatcher>();

        var response = await dispatcher.DispatchAsync(
            CreateTransaction(scope.ServiceProvider),
            Request(
                JmapRequestBuilder.Invocation(
                    "Calendar/get",
                    "c1",
                    new Dictionary<string, object?>
                    {
                        ["accountId"] = "account1",
                        ["ids"] = new[] { "calendar1" },
                        ["properties"] = new[] { "name" }
                    })));

        var responseJson = JsonSerializer.Serialize(response.MethodResponses[0].Arguments);
        using var document = JsonDocument.Parse(responseJson);
        var calendar = document.RootElement.GetProperty("list")[0];

        await Assert.That(response.MethodResponses[0].Name).IsEqualTo("Calendar/get");
        using var multiple = Assert.Multiple();
        await Assert.That(calendar.TryGetProperty("id", out _)).IsTrue();
        await Assert.That(calendar.TryGetProperty("name", out _)).IsTrue();
        await Assert.That(calendar.TryGetProperty("color", out _)).IsFalse();
        await Assert.That(calendar.TryGetProperty("myRights", out _)).IsFalse();
    }

    private static JmapExecutionContext CreateExecutionContext(IReadOnlySet<JmapId> accountIds)
    {
        return new JmapExecutionContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity("Test")),
            PrincipalId = new JmapId("principal1"),
            AccountIds = accountIds,
            Transaction = CreateTransaction(new ServiceCollection().BuildServiceProvider())
        };
    }

    private static JMAP.Net.Common.Protocol.JmapRequest Request(
        params JMAP.Net.Common.Protocol.Invocation[] invocations)
    {
        return new JMAP.Net.Common.Protocol.JmapRequest
        {
            Using = [JMAP.Net.Capabilities.Calendars.CalendarCapability.CapabilityUri],
            MethodCalls = invocations.ToList(),
            CreatedIds = null
        };
    }

    private sealed class TestUserContextProvider : IJmapUserContextProvider
    {
        public ValueTask<JmapExecutionContext> GetContextAsync(
            JmapTransaction transaction,
            CancellationToken cancellationToken = default)
        {
            return ValueTask.FromResult(new JmapExecutionContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity("Test")),
                PrincipalId = new JmapId("principal1"),
                AccountIds = new HashSet<JmapId> { new("account1") },
                Transaction = transaction
            });
        }
    }

    private sealed class InMemoryCalendarStore : ICalendarStore
    {
        public ValueTask<JmapGetResult<Calendar>> GetAsync(
            JmapId accountId,
            IReadOnlyList<JmapId>? ids,
            IReadOnlyList<string>? properties,
            CancellationToken cancellationToken = default)
        {
            var calendar = CreateCalendar();
            var list = ids is null || ids.Contains(calendar.Id)
                ? new[] { calendar }.ToList()
                : [];

            var foundIds = list.Select(item => item.Id).ToHashSet();
            var notFound = ids is null
                ? []
                : ids.Where(id => !foundIds.Contains(id)).ToList();

            return ValueTask.FromResult(new JmapGetResult<Calendar>
            {
                State = "calendar-state-1",
                List = list,
                NotFound = notFound
            });
        }

        public ValueTask<JmapQueryResult> QueryAsync(
            JmapId accountId,
            object? filter,
            IReadOnlyList<Comparator>? sort,
            int position,
            JmapId? anchor,
            int anchorOffset,
            uint? limit,
            bool calculateTotal,
            CancellationToken cancellationToken = default)
        {
            return ValueTask.FromResult(new JmapQueryResult
            {
                QueryState = "calendar-state-1",
                CanCalculateChanges = true,
                Position = 0,
                Ids = [new JmapId("calendar1")],
                Total = calculateTotal ? 1 : null
            });
        }

        public ValueTask<JmapChangesResult> ChangesAsync(
            JmapId accountId,
            string sinceState,
            uint? maxChanges,
            CancellationToken cancellationToken = default)
        {
            return ValueTask.FromResult(new JmapChangesResult
            {
                OldState = sinceState,
                NewState = "calendar-state-1",
                HasMoreChanges = false,
                Created = sinceState == "calendar-state-0" ? [new JmapId("calendar1")] : [],
                Updated = [],
                Destroyed = []
            });
        }

        private static Calendar CreateCalendar()
        {
            return new Calendar
            {
                Id = new JmapId("calendar1"),
                Name = "Work",
                Color = "#2f80ed",
                IsSubscribed = true,
                IsDefault = true,
                IncludeInAvailability = CalendarAvailabilityInclusion.All,
                MyRights = new CalendarRights
                {
                    MayReadFreeBusy = true,
                    MayReadItems = true,
                    MayWriteAll = true,
                    MayWriteOwn = true,
                    MayUpdatePrivate = true,
                    MayRSVP = true,
                    MayAdmin = true,
                    MayDelete = true
                }
            };
        }
    }
}
