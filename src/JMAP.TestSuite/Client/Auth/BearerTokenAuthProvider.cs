using System.Net.Http.Headers;

namespace JMAP.TestSuite.Client.Auth;

public sealed class BearerTokenAuthProvider(string token) : IJmapAuthProvider
{
    public ValueTask ApplyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return ValueTask.CompletedTask;
    }
}
