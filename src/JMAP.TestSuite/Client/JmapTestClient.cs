using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using JMAP.TestSuite.Client.Auth;

namespace JMAP.TestSuite.Client;

public sealed class JmapTestClient
{
    private readonly HttpClient _httpClient;
    private readonly IJmapAuthProvider _authProvider;
    private readonly JmapClientOptions _options;

    public JmapTestClient(HttpClient httpClient, IJmapAuthProvider authProvider, JmapClientOptions options)
    {
        _httpClient = httpClient;
        _authProvider = authProvider;
        _options = options;
        _httpClient.Timeout = options.Timeout;
    }

    public Task<JmapHttpResult> GetSessionAsync(CancellationToken cancellationToken)
    {
        if (_options.SessionUrl is null)
        {
            throw new InvalidOperationException("Session URL is required for session steps.");
        }

        return SendHttpAsync(HttpMethod.Get, _options.SessionUrl, requestBody: null, cancellationToken);
    }

    public Task<JmapHttpResult> SendAsync(JsonObject request, CancellationToken cancellationToken)
    {
        var body = request.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
        return SendHttpAsync(HttpMethod.Post, _options.ApiUrl, body, cancellationToken);
    }

    public Task<JmapHttpResult> SendRawAsync(string body, CancellationToken cancellationToken)
        => SendHttpAsync(HttpMethod.Post, _options.ApiUrl, body, cancellationToken);

    private async Task<JmapHttpResult> SendHttpAsync(
        HttpMethod method,
        Uri url,
        string? requestBody,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, url);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (requestBody is not null)
        {
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        }

        await _authProvider.ApplyAsync(request, cancellationToken).ConfigureAwait(false);

        var stopwatch = Stopwatch.StartNew();
        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        stopwatch.Stop();

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        JsonNode? responseJson = null;
        string? parseError = null;

        if (!string.IsNullOrWhiteSpace(responseBody))
        {
            try
            {
                responseJson = JsonNode.Parse(responseBody);
            }
            catch (JsonException ex)
            {
                parseError = ex.Message;
            }
        }

        return new JmapHttpResult
        {
            Method = method,
            Url = url,
            StatusCode = (int)response.StatusCode,
            RequestBody = requestBody,
            ResponseBody = responseBody,
            ResponseJson = responseJson,
            JsonParseError = parseError,
            Duration = stopwatch.Elapsed,
            ResponseHeaders = CollectHeaders(response)
        };
    }

    private static IReadOnlyDictionary<string, IReadOnlyList<string>> CollectHeaders(HttpResponseMessage response)
    {
        var headers = new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase);

        foreach (var header in response.Headers)
        {
            headers[header.Key] = header.Value.ToList();
        }

        foreach (var header in response.Content.Headers)
        {
            headers[header.Key] = header.Value.ToList();
        }

        return headers;
    }
}
