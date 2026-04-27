using System.Text.Json;
using JMAP.Net.Common.Errors;
using JMAP.Net.Common.Protocol;
using JMAP.Net.Hosting.Configuration;
using JMAP.Net.Hosting.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace JMAP.Net.Hosting.Internal;

/// <summary>
/// Implements the default ASP.NET Core endpoint handlers used by <c>MapJmap()</c>.
/// </summary>
internal static class JmapEndpointHandlers
{
    public static async Task HandleSessionAsync(
        HttpContext httpContext,
        IJmapSessionProvider sessionProvider,
        IOptionsMonitor<JmapAspNetCoreOptions> options)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(sessionProvider);
        ArgumentNullException.ThrowIfNull(options);

        var transaction = CreateTransaction(httpContext);
        httpContext.Features.Set(new JmapAspNetCoreFeature { Transaction = transaction });

        if (!await EnsureHttpsRequirementAsync(httpContext, options.CurrentValue, httpContext.RequestAborted))
        {
            return;
        }

        transaction.Session = await sessionProvider.GetSessionAsync(transaction, httpContext.RequestAborted);

        await WriteJsonAsync(httpContext, transaction.Session, options.CurrentValue, httpContext.RequestAborted);
    }

    public static async Task HandleApiAsync(
        HttpContext httpContext,
        IJmapSessionProvider sessionProvider,
        IJmapRequestDispatcher dispatcher,
        IJmapRequestConcurrencyLimiter concurrencyLimiter,
        IOptionsMonitor<JmapServerOptions> serverOptions,
        IOptionsMonitor<JmapAspNetCoreOptions> aspNetCoreOptions)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(sessionProvider);
        ArgumentNullException.ThrowIfNull(dispatcher);
        ArgumentNullException.ThrowIfNull(concurrencyLimiter);
        ArgumentNullException.ThrowIfNull(serverOptions);
        ArgumentNullException.ThrowIfNull(aspNetCoreOptions);

        var transaction = CreateTransaction(httpContext);
        httpContext.Features.Set(new JmapAspNetCoreFeature { Transaction = transaction });

        if (!await EnsureHttpsRequirementAsync(httpContext, aspNetCoreOptions.CurrentValue, httpContext.RequestAborted))
        {
            return;
        }

        var serverConfiguration = serverOptions.CurrentValue;

        if (!await ValidateApiRequestEnvelopeAsync(
                httpContext,
                serverConfiguration,
                aspNetCoreOptions.CurrentValue,
                httpContext.RequestAborted))
        {
            return;
        }

        using var concurrencyLease = await concurrencyLimiter.TryAcquireAsync(
            serverConfiguration.MaxConcurrentRequests,
            httpContext.RequestAborted);

        if (concurrencyLease is null)
        {
            await WriteProblemDetailsAsync(
                httpContext,
                new ProblemDetails
                {
                    Type = ProblemDetailsType.Limit,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "The server is already processing as many concurrent API requests as it allows.",
                    Limit = "maxConcurrentRequests"
                },
                aspNetCoreOptions.CurrentValue,
                httpContext.RequestAborted);
            return;
        }

        transaction.Session = await sessionProvider.GetSessionAsync(transaction, httpContext.RequestAborted);

        JmapRequest? request;

        JsonDocument document;

        try
        {
            document = await JsonDocument.ParseAsync(
                httpContext.Request.Body,
                cancellationToken: httpContext.RequestAborted);
        }
        catch (JsonException)
        {
            await WriteProblemDetailsAsync(
                httpContext,
                new ProblemDetails
                {
                    Type = ProblemDetailsType.NotJson,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "The request body was not valid JSON."
                },
                aspNetCoreOptions.CurrentValue,
                httpContext.RequestAborted);
            return;
        }

        try
        {
            request = document.Deserialize<JmapRequest>();
        }
        catch (JsonException)
        {
            await WriteProblemDetailsAsync(
                httpContext,
                new ProblemDetails
                {
                    Type = ProblemDetailsType.NotRequest,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "The JSON payload was valid but was not a valid JMAP request object."
                },
                aspNetCoreOptions.CurrentValue,
                httpContext.RequestAborted);
            return;
        }

        if (request is null)
        {
            await WriteProblemDetailsAsync(
                httpContext,
                new ProblemDetails
                {
                    Type = ProblemDetailsType.NotRequest,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "The JSON payload was valid but did not contain a JMAP request object."
                },
                aspNetCoreOptions.CurrentValue,
                httpContext.RequestAborted);
            return;
        }

        if (!await ValidateRequestShapeAsync(httpContext, request, aspNetCoreOptions.CurrentValue, httpContext.RequestAborted))
        {
            return;
        }

        if (request.MethodCalls.Count > serverConfiguration.MaxCallsInRequest)
        {
            await WriteProblemDetailsAsync(
                httpContext,
                new ProblemDetails
                {
                    Type = ProblemDetailsType.Limit,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "The request contains more method calls than this server is willing to process.",
                    Limit = "maxCallsInRequest"
                },
                aspNetCoreOptions.CurrentValue,
                httpContext.RequestAborted);
            return;
        }

        var unknownCapability = request.Using.FirstOrDefault(
            capability => !transaction.Session.Capabilities.ContainsKey(capability));

        if (unknownCapability is not null)
        {
            await WriteProblemDetailsAsync(
                httpContext,
                new ProblemDetails
                {
                    Type = ProblemDetailsType.UnknownCapability,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = $"The capability '{unknownCapability}' is not supported by this server."
                },
                aspNetCoreOptions.CurrentValue,
                httpContext.RequestAborted);
            return;
        }

        var response = await dispatcher.DispatchAsync(transaction, request, httpContext.RequestAborted);

        await WriteJsonAsync(httpContext, response, aspNetCoreOptions.CurrentValue, httpContext.RequestAborted);
    }

    private static async Task<bool> ValidateApiRequestEnvelopeAsync(
        HttpContext httpContext,
        JmapServerOptions serverOptions,
        JmapAspNetCoreOptions aspNetCoreOptions,
        CancellationToken cancellationToken)
    {
        if (!IsJsonContentType(httpContext.Request.ContentType))
        {
            await WriteProblemDetailsAsync(
                httpContext,
                new ProblemDetails
                {
                    Type = ProblemDetailsType.NotRequest,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "The request Content-Type must be application/json."
                },
                aspNetCoreOptions,
                cancellationToken);
            return false;
        }

        if (httpContext.Request.ContentLength > serverOptions.MaxSizeRequest)
        {
            await WriteProblemDetailsAsync(
                httpContext,
                new ProblemDetails
                {
                    Type = ProblemDetailsType.Limit,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "The request body is larger than this server is willing to process.",
                    Limit = "maxSizeRequest"
                },
                aspNetCoreOptions,
                cancellationToken);
            return false;
        }

        return true;
    }

    private static bool IsJsonContentType(string? contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
        {
            return false;
        }

        var mediaType = contentType.Split(';', 2)[0].Trim();
        return string.Equals(mediaType, "application/json", StringComparison.OrdinalIgnoreCase);
    }

    private static async Task<bool> ValidateRequestShapeAsync(
        HttpContext httpContext,
        JmapRequest request,
        JmapAspNetCoreOptions options,
        CancellationToken cancellationToken)
    {
        if (request.Using is null
            || request.MethodCalls is null
            || request.Using.Any(string.IsNullOrWhiteSpace)
            || request.MethodCalls.Any(invocation =>
                invocation is null
                || string.IsNullOrWhiteSpace(invocation.Name)
                || invocation.Arguments is null
                || string.IsNullOrWhiteSpace(invocation.MethodCallId)))
        {
            await WriteProblemDetailsAsync(
                httpContext,
                new ProblemDetails
                {
                    Type = ProblemDetailsType.NotRequest,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "The JSON payload was valid but was not a valid JMAP request object."
                },
                options,
                cancellationToken);
            return false;
        }

        return true;
    }

    private static JmapTransaction CreateTransaction(HttpContext httpContext)
    {
        return new JmapTransaction
        {
            Services = httpContext.RequestServices,
            HttpContext = httpContext
        };
    }

    private static async Task<bool> EnsureHttpsRequirementAsync(
        HttpContext httpContext,
        JmapAspNetCoreOptions options,
        CancellationToken cancellationToken)
    {
        if (!options.RequireHttps || httpContext.Request.IsHttps)
        {
            return true;
        }

        await WriteProblemDetailsAsync(
            httpContext,
            new ProblemDetails
            {
                Type = ProblemDetailsType.NotRequest,
                Status = StatusCodes.Status400BadRequest,
                Detail = "HTTPS is required for the configured JMAP endpoints."
            },
            options,
            cancellationToken);

        return false;
    }

    private static async Task WriteProblemDetailsAsync(
        HttpContext httpContext,
        ProblemDetails problem,
        JmapAspNetCoreOptions options,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = problem.Status;

        await WriteJsonAsync(httpContext, problem, options, cancellationToken);
    }

    private static async Task WriteJsonAsync<T>(
        HttpContext httpContext,
        T value,
        JmapAspNetCoreOptions options,
        CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json; charset=utf-8";

        await JsonSerializer.SerializeAsync(
            httpContext.Response.Body,
            value,
            new JsonSerializerOptions
            {
                WriteIndented = options.WriteIndentedResponses
            },
            cancellationToken);
    }
}
