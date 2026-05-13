namespace JMAP.TestSuite.Client.Auth;

public interface IJmapAuthProvider
{
    ValueTask ApplyAsync(HttpRequestMessage request, CancellationToken cancellationToken);
}
