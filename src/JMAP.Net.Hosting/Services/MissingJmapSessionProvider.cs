using JMAP.Net.Common.Session;

namespace JMAP.Net.Hosting.Services;

/// <summary>
/// Represents the fallback session provider used when no application provider was registered.
/// </summary>
internal sealed class MissingJmapSessionProvider : IJmapSessionProvider
{
    public ValueTask<JmapSession> GetSessionAsync(JmapTransaction transaction, CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException(
            "No IJmapSessionProvider has been registered. Register one via AddSessionProvider<TProvider>() or by adding IJmapSessionProvider to the service collection.");
    }
}
