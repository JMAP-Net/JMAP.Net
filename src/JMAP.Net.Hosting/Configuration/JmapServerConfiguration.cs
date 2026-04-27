using Microsoft.Extensions.Options;

namespace JMAP.Net.Hosting.Configuration;

/// <summary>
/// Validates and normalizes the configured JMAP server options.
/// </summary>
internal sealed class JmapServerConfiguration : IPostConfigureOptions<JmapServerOptions>
{
    /// <inheritdoc />
    public void PostConfigure(string? name, JmapServerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.ApiPath = NormalizePath(options.ApiPath, nameof(options.ApiPath));
        options.SessionPath = NormalizePath(options.SessionPath, nameof(options.SessionPath));

        if (string.Equals(options.ApiPath, options.SessionPath, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("The JMAP API path and session path must be different.");
        }

        if (options.MaxParallelMethodCalls < 1)
        {
            throw new InvalidOperationException($"{nameof(options.MaxParallelMethodCalls)} must be greater than zero.");
        }

        if (options.MaxCallsInRequest < 1)
        {
            throw new InvalidOperationException($"{nameof(options.MaxCallsInRequest)} must be greater than zero.");
        }
    }

    private static string NormalizePath(string path, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new InvalidOperationException($"{parameterName} must not be null, empty, or whitespace.");
        }

        if (!path.StartsWith('/'))
        {
            throw new InvalidOperationException($"{parameterName} must be an absolute path starting with '/'.");
        }

        if (path.Length > 1)
        {
            path = path.TrimEnd('/');
        }

        return path;
    }
}
