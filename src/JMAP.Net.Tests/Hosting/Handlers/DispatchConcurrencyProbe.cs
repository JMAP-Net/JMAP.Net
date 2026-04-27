namespace JMAP.Net.Tests.Hosting.Handlers;

internal sealed class DispatchConcurrencyProbe
{
    private int _currentConcurrency;
    private int _maxObservedConcurrency;

    public int MaxObservedConcurrency => _maxObservedConcurrency;

    public async ValueTask RunAsync(int delayMilliseconds, CancellationToken cancellationToken)
    {
        var current = Interlocked.Increment(ref _currentConcurrency);

        try
        {
            UpdateMax(current);
            await Task.Delay(delayMilliseconds, cancellationToken);
        }
        finally
        {
            Interlocked.Decrement(ref _currentConcurrency);
        }
    }

    private void UpdateMax(int current)
    {
        while (true)
        {
            var observed = Volatile.Read(ref _maxObservedConcurrency);

            if (current <= observed)
            {
                return;
            }

            if (Interlocked.CompareExchange(ref _maxObservedConcurrency, current, observed) == observed)
            {
                return;
            }
        }
    }
}
