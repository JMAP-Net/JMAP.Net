using System.ComponentModel;
using JMAP.Net.Hosting.Configuration;
using JMAP.Net.Hosting.Engine;
using JMAP.Net.Hosting.Engine.Calendar;
using JMAP.Net.Hosting.Engine.Principal;
using JMAP.Net.Hosting.Internal.Handlers;
using JMAP.Net.Hosting.Services;
using JMAP.Net.Persistence.Calendars;
using JMAP.Net.Persistence.Sharing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JMAP.Net.Hosting.Builders;

/// <summary>
/// Exposes the methods used to configure the JMAP server services.
/// </summary>
public sealed class JmapServerBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JmapServerBuilder" /> class.
    /// </summary>
    /// <param name="services">The services collection.</param>
    public JmapServerBuilder(IServiceCollection services)
        => Services = services ?? throw new ArgumentNullException(nameof(services));

    /// <summary>
    /// Gets the services collection.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public IServiceCollection Services { get; }

    /// <summary>
    /// Applies additional server options configuration.
    /// </summary>
    /// <param name="configuration">The configuration delegate.</param>
    /// <returns>The current builder instance.</returns>
    public JmapServerBuilder Configure(Action<JmapServerOptions> configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        Services.Configure(configuration);

        return this;
    }

    /// <summary>
    /// Registers the session provider used to serve session documents.
    /// </summary>
    /// <typeparam name="TProvider">The session provider type.</typeparam>
    /// <returns>The current builder instance.</returns>
    public JmapServerBuilder AddSessionProvider<TProvider>()
        where TProvider : class, IJmapSessionProvider
    {
        Services.Replace(ServiceDescriptor.Scoped<IJmapSessionProvider, TProvider>());

        return this;
    }

    /// <summary>
    /// Registers a method handler that can process a JMAP method call.
    /// </summary>
    /// <typeparam name="THandler">The handler type.</typeparam>
    /// <returns>The current builder instance.</returns>
    public JmapServerBuilder AddMethodHandler<THandler>()
        where THandler : class, IJmapMethodHandler
    {
        Services.TryAddEnumerable(ServiceDescriptor.Scoped<IJmapMethodHandler, THandler>());

        return this;
    }

    /// <summary>
    /// Registers the Principal engine slice and the Principal/get method handler.
    /// </summary>
    /// <typeparam name="TPrincipalStore">The Principal store implementation type.</typeparam>
    /// <typeparam name="TUserContextProvider">The user context provider implementation type.</typeparam>
    /// <returns>The current builder instance.</returns>
    public JmapServerBuilder AddPrincipalEngine<TPrincipalStore, TUserContextProvider>()
        where TPrincipalStore : class, IPrincipalStore
        where TUserContextProvider : class, IJmapUserContextProvider
    {
        Services.TryAddScoped<IPrincipalStore, TPrincipalStore>();
        Services.TryAddScoped<IJmapUserContextProvider, TUserContextProvider>();
        Services.TryAddScoped<IPrincipalEngine, PrincipalEngine>();
        Services.TryAddEnumerable(ServiceDescriptor.Scoped<IJmapMethodHandler, PrincipalGetHandler>());

        return this;
    }

    /// <summary>
    /// Registers the Calendar engine slice and Calendar/get, Calendar/query and Calendar/changes method handlers.
    /// </summary>
    /// <typeparam name="TCalendarStore">The Calendar store implementation type.</typeparam>
    /// <typeparam name="TUserContextProvider">The user context provider implementation type.</typeparam>
    /// <returns>The current builder instance.</returns>
    public JmapServerBuilder AddCalendarEngine<TCalendarStore, TUserContextProvider>()
        where TCalendarStore : class, ICalendarStore
        where TUserContextProvider : class, IJmapUserContextProvider
    {
        Services.TryAddScoped<ICalendarStore, TCalendarStore>();
        Services.TryAddScoped<IJmapUserContextProvider, TUserContextProvider>();
        Services.TryAddScoped<ICalendarEngine, CalendarEngine>();
        Services.TryAddEnumerable(ServiceDescriptor.Scoped<IJmapMethodHandler, CalendarGetHandler>());
        Services.TryAddEnumerable(ServiceDescriptor.Scoped<IJmapMethodHandler, CalendarQueryHandler>());
        Services.TryAddEnumerable(ServiceDescriptor.Scoped<IJmapMethodHandler, CalendarChangesHandler>());

        return this;
    }

    /// <summary>
    /// Sets the API endpoint path used for JMAP method calls.
    /// </summary>
    /// <param name="path">The absolute path.</param>
    /// <returns>The current builder instance.</returns>
    public JmapServerBuilder SetApiPath(string path)
        => Configure(options => options.ApiPath = path);

    /// <summary>
    /// Sets the session endpoint path used for the JMAP session document.
    /// </summary>
    /// <param name="path">The absolute path.</param>
    /// <returns>The current builder instance.</returns>
    public JmapServerBuilder SetSessionPath(string path)
        => Configure(options => options.SessionPath = path);

    /// <summary>
    /// Enables parallel dispatch for method handlers that declare compatible execution metadata.
    /// </summary>
    /// <param name="maxDegreeOfParallelism">The maximum number of method calls to run at the same time.</param>
    /// <returns>The current builder instance.</returns>
    public JmapServerBuilder EnableParallelDispatch(int maxDegreeOfParallelism)
    {
        if (maxDegreeOfParallelism < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maxDegreeOfParallelism),
                "The maximum degree of parallelism must be greater than zero.");
        }

        return Configure(options =>
        {
            options.EnableParallelMethodDispatch = true;
            options.MaxParallelMethodCalls = maxDegreeOfParallelism;
        });
    }

    /// <summary>
    /// Sets the maximum number of method calls accepted in a single JMAP request.
    /// </summary>
    /// <param name="maxCalls">The maximum number of method calls.</param>
    /// <returns>The current builder instance.</returns>
    public JmapServerBuilder SetMaxCallsInRequest(int maxCalls)
    {
        if (maxCalls < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maxCalls),
                "The maximum number of method calls must be greater than zero.");
        }

        return Configure(options => options.MaxCallsInRequest = maxCalls);
    }

    /// <summary>
    /// Sets the maximum number of concurrent API requests accepted by the server.
    /// </summary>
    /// <param name="maxRequests">The maximum number of concurrent API requests.</param>
    /// <returns>The current builder instance.</returns>
    public JmapServerBuilder SetMaxConcurrentRequests(int maxRequests)
    {
        if (maxRequests < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maxRequests),
                "The maximum number of concurrent requests must be greater than zero.");
        }

        return Configure(options => options.MaxConcurrentRequests = maxRequests);
    }

    /// <summary>
    /// Sets the maximum request body size accepted by the API endpoint.
    /// </summary>
    /// <param name="maxSize">The maximum request body size in octets.</param>
    /// <returns>The current builder instance.</returns>
    public JmapServerBuilder SetMaxSizeRequest(long maxSize)
    {
        if (maxSize < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maxSize),
                "The maximum request size must be greater than zero.");
        }

        return Configure(options => options.MaxSizeRequest = maxSize);
    }

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object? obj) => base.Equals(obj);

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string? ToString() => base.ToString();
}
