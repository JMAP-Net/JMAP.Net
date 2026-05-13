using System.Security.Claims;
using System.Text.Json;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Capabilities.Sharing;
using JMAP.Net.Capabilities.Sharing.Methods.Principal;
using JMAP.Net.Hosting;
using JMAP.Net.Hosting.Engine;
using JMAP.Net.Hosting.Engine.Principal;
using JMAP.Net.Hosting.Services;
using JMAP.Net.Tests.Hosting.Infrastructure;
using JMAP.Net.Tests.Sharing;
using Microsoft.Extensions.DependencyInjection;

namespace JMAP.Net.Tests.Hosting;

public class PrincipalEngineTests : JmapHostingTestBase
{
    [Test]
    public async Task PrincipalEngine_GetAsync_WhenIdsAreSpecified_ShouldReturnListAndNotFound()
    {
        var store = new InMemoryPrincipalStore(
            "principal-state-1",
            [PrincipalJsonTests.CreatePrincipal()]);
        var engine = new PrincipalEngine(store);

        var response = await engine.GetAsync(
            CreateExecutionContext(new HashSet<JmapId> { new("principal-account") }),
            new PrincipalGetRequest
            {
                AccountId = new JmapId("principal-account"),
                Ids =
                [
                    new JmapId("principal1"),
                    new JmapId("missing-principal")
                ]
            });

        await Assert.That(response.State).IsEqualTo("principal-state-1");
        using var _ = Assert.Multiple();
        await Assert.That(response.List.Count).IsEqualTo(1);
        await Assert.That(response.List[0].Id).IsEqualTo(new JmapId("principal1"));
        await Assert.That(response.NotFound.Count).IsEqualTo(1);
        await Assert.That(response.NotFound[0]).IsEqualTo(new JmapId("missing-principal"));
    }

    [Test]
    public async Task PrincipalEngine_GetAsync_WhenAccountIsNotAllowed_ShouldThrowAccountNotFound()
    {
        var store = new InMemoryPrincipalStore(
            "principal-state-1",
            [PrincipalJsonTests.CreatePrincipal()]);
        var engine = new PrincipalEngine(store);

        var act = async () => await engine.GetAsync(
            CreateExecutionContext(new HashSet<JmapId> { new("other-account") }),
            new PrincipalGetRequest
            {
                AccountId = new JmapId("principal-account")
            });

        var exception = await Assert.ThrowsAsync<JmapMethodException>(act);
        await Assert.That(exception!.ErrorType).IsEqualTo("accountNotFound");
    }

    [Test]
    public async Task Dispatcher_WhenPrincipalGetIsRegistered_ShouldReturnPrincipalGetResponse()
    {
        await using var scope = CreateServiceScope(server =>
            server.AddPrincipalEngine<InMemoryPrincipalStore, TestUserContextProvider>());
        var dispatcher = scope.ServiceProvider.GetRequiredService<IJmapRequestDispatcher>();

        var response = await dispatcher.DispatchAsync(
            CreateTransaction(scope.ServiceProvider),
            PrincipalRequest(
                JmapRequestBuilder.Invocation(
                    "Principal/get",
                    "c1",
                    new Dictionary<string, object?>
                    {
                        ["accountId"] = "principal-account",
                        ["ids"] = new[] { "principal1", "missing-principal" }
                    })));

        var responseJson = JsonSerializer.Serialize(response.MethodResponses[0].Arguments);
        var principalResponse = JsonSerializer.Deserialize<PrincipalGetResponse>(responseJson);

        await Assert.That(response.MethodResponses[0].Name).IsEqualTo("Principal/get");
        await Assert.That(principalResponse).IsNotNull();
        using var _ = Assert.Multiple();
        await Assert.That(principalResponse!.List[0].Id).IsEqualTo(new JmapId("principal1"));
        await Assert.That(principalResponse.NotFound[0]).IsEqualTo(new JmapId("missing-principal"));
    }

    [Test]
    public async Task Dispatcher_WhenPrincipalGetUsesUnavailableAccount_ShouldReturnAccountNotFoundError()
    {
        await using var scope = CreateServiceScope(server =>
            server.AddPrincipalEngine<InMemoryPrincipalStore, TestUserContextProvider>());
        var dispatcher = scope.ServiceProvider.GetRequiredService<IJmapRequestDispatcher>();

        var response = await dispatcher.DispatchAsync(
            CreateTransaction(scope.ServiceProvider),
            PrincipalRequest(
                JmapRequestBuilder.Invocation(
                    "Principal/get",
                    "c1",
                    new Dictionary<string, object?>
                    {
                        ["accountId"] = "other-account"
                    })));

        await Assert.That(response.MethodResponses[0].Name).IsEqualTo("error");
        await Assert.That(response.MethodResponses[0].Arguments["type"]).IsEqualTo("accountNotFound");
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

    private static JMAP.Net.Common.Protocol.JmapRequest PrincipalRequest(
        params JMAP.Net.Common.Protocol.Invocation[] invocations)
    {
        return new JMAP.Net.Common.Protocol.JmapRequest
        {
            Using = [PrincipalCapability.CapabilityUri],
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
                AccountIds = new HashSet<JmapId> { new("principal-account") },
                Transaction = transaction
            });
        }
    }

    private sealed class InMemoryPrincipalStore : IPrincipalStore
    {
        private readonly string? _state;
        private readonly IReadOnlyList<JMAP.Net.Capabilities.Sharing.Types.Principal> _principals;

        public InMemoryPrincipalStore()
            : this("principal-state-1", [PrincipalJsonTests.CreatePrincipal()])
        {
        }

        public InMemoryPrincipalStore(
            string? state,
            IReadOnlyList<JMAP.Net.Capabilities.Sharing.Types.Principal> principals)
        {
            _state = state;
            _principals = principals;
        }

        public ValueTask<string?> GetStateAsync(
            JmapId accountId,
            CancellationToken cancellationToken = default)
            => ValueTask.FromResult(_state);

        public ValueTask<IReadOnlyList<JMAP.Net.Capabilities.Sharing.Types.Principal>> GetAsync(
            JmapId accountId,
            IReadOnlyList<JmapId>? ids,
            CancellationToken cancellationToken = default)
        {
            if (ids is null)
            {
                return ValueTask.FromResult(_principals);
            }

            var requested = ids.ToHashSet();
            var principals = _principals
                .Where(principal => requested.Contains(principal.Id))
                .ToList();

            return ValueTask.FromResult<IReadOnlyList<JMAP.Net.Capabilities.Sharing.Types.Principal>>(principals);
        }
    }
}
