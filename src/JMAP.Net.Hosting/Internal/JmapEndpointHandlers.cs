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
        IOptionsMonitor<JmapAspNetCoreOptions> aspNetCoreOptions)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(sessionProvider);
        ArgumentNullException.ThrowIfNull(dispatcher);
        ArgumentNullException.ThrowIfNull(aspNetCoreOptions);

        var transaction = CreateTransaction(httpContext);
        httpContext.Features.Set(new JmapAspNetCoreFeature { Transaction = transaction });

        if (!await EnsureHttpsRequirementAsync(httpContext, aspNetCoreOptions.CurrentValue, httpContext.RequestAborted))
        {
            return;
        }

        transaction.Session = await sessionProvider.GetSessionAsync(transaction, httpContext.RequestAborted);

        JmapRequest? request;

        try
        {
            request = await JsonSerializer.DeserializeAsync<JmapRequest>(
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
