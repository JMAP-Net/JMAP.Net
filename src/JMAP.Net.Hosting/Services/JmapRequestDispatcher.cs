using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Common.Errors;
using JMAP.Net.Common.Protocol;
using JMAP.Net.Hosting.Configuration;
using Microsoft.Extensions.Options;

namespace JMAP.Net.Hosting.Services;

/// <summary>
/// Implements the default in-memory JMAP request dispatcher.
/// </summary>
public sealed class JmapRequestDispatcher : IJmapRequestDispatcher
{
    private readonly IReadOnlyDictionary<string, IJmapMethodHandler> _handlers;
    private readonly IOptionsMonitor<JmapServerOptions> _options;
    private readonly IJmapSessionProvider _sessionProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="JmapRequestDispatcher" /> class.
    /// </summary>
    /// <param name="handlers">The registered method handlers.</param>
    /// <param name="sessionProvider">The session provider.</param>
    /// <param name="options">The server options.</param>
    public JmapRequestDispatcher(
        IEnumerable<IJmapMethodHandler> handlers,
        IJmapSessionProvider sessionProvider,
        IOptionsMonitor<JmapServerOptions> options)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _handlers = handlers
            .GroupBy(handler => handler.MethodName, StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.Last(), StringComparer.Ordinal);
    }

    /// <inheritdoc />
    public async ValueTask<JmapResponse> DispatchAsync(
        JmapTransaction transaction,
        JmapRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        ArgumentNullException.ThrowIfNull(request);

        transaction.Session ??= await _sessionProvider.GetSessionAsync(transaction, cancellationToken);

        var options = _options.CurrentValue;

        Invocation[] responses = options.EnableParallelMethodDispatch
            ? await DispatchInParallelAsync(transaction, request, options, cancellationToken)
            : await DispatchSequentiallyAsync(transaction, request, cancellationToken);

        return new JmapResponse
        {
            MethodResponses = responses.ToList(),
            CreatedIds = request.CreatedIds is null ? null : new Dictionary<JmapId, JmapId>(request.CreatedIds),
            SessionState = transaction.Session.State
        };
    }

    private async ValueTask<Invocation[]> DispatchSequentiallyAsync(
        JmapTransaction transaction,
        JmapRequest request,
        CancellationToken cancellationToken)
    {
        var responses = new Invocation[request.MethodCalls.Count];

        for (var index = 0; index < request.MethodCalls.Count; index++)
        {
            responses[index] = await DispatchInvocationAsync(transaction, request, request.MethodCalls[index], cancellationToken);
        }

        return responses;
    }

    private async ValueTask<Invocation[]> DispatchInParallelAsync(
        JmapTransaction transaction,
        JmapRequest request,
        JmapServerOptions options,
        CancellationToken cancellationToken)
    {
        var responses = new Invocation[request.MethodCalls.Count];
        var batches = CreateExecutionBatches(transaction, request);
        using var throttler = new SemaphoreSlim(options.MaxParallelMethodCalls);

        foreach (var batch in batches)
        {
            var tasks = batch.Select(async item =>
            {
                await throttler.WaitAsync(cancellationToken);

                try
                {
                    responses[item.Index] = await DispatchInvocationAsync(
                        transaction,
                        request,
                        item.Invocation,
                        cancellationToken);
                }
                finally
                {
                    throttler.Release();
                }
            });

            await Task.WhenAll(tasks);
        }

        return responses;
    }

    private List<List<ScheduledInvocation>> CreateExecutionBatches(JmapTransaction transaction, JmapRequest request)
    {
        var batches = new List<List<ScheduledInvocation>>();
        var currentBatch = new List<ScheduledInvocation>();

        for (var index = 0; index < request.MethodCalls.Count; index++)
        {
            var invocation = request.MethodCalls[index];
            var item = CreateScheduledInvocation(transaction, request, invocation, index);

            if (!CanRunInParallel(item) || HasResultReference(invocation))
            {
                AddCurrentBatch();
                batches.Add([item]);
                continue;
            }

            if (ConflictsWithBatch(item, currentBatch))
            {
                AddCurrentBatch();
            }

            currentBatch.Add(item);
        }

        AddCurrentBatch();

        return batches;

        void AddCurrentBatch()
        {
            if (currentBatch.Count == 0)
            {
                return;
            }

            batches.Add(currentBatch);
            currentBatch = [];
        }
    }

    private ScheduledInvocation CreateScheduledInvocation(
        JmapTransaction transaction,
        JmapRequest request,
        Invocation invocation,
        int index)
    {
        if (!_handlers.TryGetValue(invocation.Name, out var handler)
            || handler is not IJmapMethodExecutionMetadata metadata)
        {
            return new ScheduledInvocation(
                index,
                invocation,
                JmapMethodExecutionMode.Sequential,
                null);
        }

        var context = new JmapMethodContext
        {
            Transaction = transaction,
            Request = request,
            Invocation = invocation
        };

        var executionMode = metadata.ExecutionMode;
        var concurrencyKey = metadata.GetConcurrencyKey(context);

        if (executionMode == JmapMethodExecutionMode.ExclusiveWrite && concurrencyKey is null)
        {
            executionMode = JmapMethodExecutionMode.Sequential;
        }

        return new ScheduledInvocation(
            index,
            invocation,
            executionMode,
            concurrencyKey);
    }

    private async ValueTask<Invocation> DispatchInvocationAsync(
        JmapTransaction transaction,
        JmapRequest request,
        Invocation invocation,
        CancellationToken cancellationToken)
    {
        if (!_handlers.TryGetValue(invocation.Name, out var handler))
        {
            return CreateErrorInvocation(
                invocation.MethodCallId,
                JmapErrorType.UnknownMethod,
                $"The method '{invocation.Name}' is not registered.");
        }

        try
        {
            var result = await handler.HandleAsync(
                new JmapMethodContext
                {
                    Transaction = transaction,
                    Request = request,
                    Invocation = invocation
                },
                cancellationToken);

            return new Invocation
            {
                Name = result.Name,
                Arguments = result.Arguments,
                MethodCallId = invocation.MethodCallId
            };
        }
        catch (Exception exception) when (!IsFatal(exception))
        {
            return CreateErrorInvocation(
                invocation.MethodCallId,
                JmapErrorType.ServerFail,
                "The server failed while processing the method call.");
        }
    }

    private static bool CanRunInParallel(ScheduledInvocation invocation)
        => invocation.ExecutionMode is JmapMethodExecutionMode.ParallelRead
            or JmapMethodExecutionMode.ExclusiveWrite;

    private static bool ConflictsWithBatch(ScheduledInvocation invocation, List<ScheduledInvocation> batch)
    {
        if (invocation.ConcurrencyKey is null)
        {
            return false;
        }

        return batch.Any(existing =>
            string.Equals(existing.ConcurrencyKey, invocation.ConcurrencyKey, StringComparison.Ordinal)
            && (existing.ExecutionMode == JmapMethodExecutionMode.ExclusiveWrite
                || invocation.ExecutionMode == JmapMethodExecutionMode.ExclusiveWrite));
    }

    private static bool HasResultReference(Invocation invocation)
        => invocation.Arguments.Values.Any(ContainsResultReference);

    private static bool ContainsResultReference(object? value)
    {
        if (value is null)
        {
            return false;
        }

        if (value is IReadOnlyDictionary<string, object?> dictionary)
        {
            return dictionary.Keys.Any(key => key.StartsWith('#'))
                || dictionary.Values.Any(ContainsResultReference);
        }

        if (value is string)
        {
            return false;
        }

        if (value is IEnumerable<object?> enumerable)
        {
            return enumerable.Any(ContainsResultReference);
        }

        return false;
    }

    private static Invocation CreateErrorInvocation(string methodCallId, string type, string description)
    {
        return new Invocation
        {
            Name = "error",
            Arguments = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["type"] = type,
                ["description"] = description
            },
            MethodCallId = methodCallId
        };
    }

    private static bool IsFatal(Exception exception)
    {
        return exception is OutOfMemoryException
            or StackOverflowException
            or AccessViolationException
            or AppDomainUnloadedException
            or BadImageFormatException
            or CannotUnloadAppDomainException
            or InvalidProgramException;
    }

    private sealed record ScheduledInvocation(
        int Index,
        Invocation Invocation,
        JmapMethodExecutionMode ExecutionMode,
        string? ConcurrencyKey);
}
