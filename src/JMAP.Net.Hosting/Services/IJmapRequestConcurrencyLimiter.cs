namespace JMAP.Net.Hosting.Services;

internal interface IJmapRequestConcurrencyLimiter
{
    ValueTask<IDisposable?> TryAcquireAsync(int maxConcurrentRequests, CancellationToken cancellationToken);
}
