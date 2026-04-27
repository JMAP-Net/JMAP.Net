using System.Collections.Concurrent;

namespace JMAP.Net.Hosting.Services;

internal sealed class JmapRequestConcurrencyLimiter : IJmapRequestConcurrencyLimiter
{
    private readonly ConcurrentDictionary<int, SemaphoreSlim> _limiters = [];

    public async ValueTask<IDisposable?> TryAcquireAsync(
        int maxConcurrentRequests,
        CancellationToken cancellationToken)
    {
        var limiter = _limiters.GetOrAdd(maxConcurrentRequests, static value => new SemaphoreSlim(value, value));

        if (!await limiter.WaitAsync(0, cancellationToken))
        {
            return null;
        }

        return new Lease(limiter);
    }

    private sealed class Lease(SemaphoreSlim limiter) : IDisposable
    {
        public void Dispose() => limiter.Release();
    }
}
