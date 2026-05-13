using JMAP.Net.Capabilities.Sharing.Methods.Principal;

namespace JMAP.Net.Hosting.Engine.Principal;

/// <summary>
/// Executes JMAP Principal method semantics.
/// </summary>
public interface IPrincipalEngine
{
    /// <summary>
    /// Executes Principal/get.
    /// </summary>
    /// <param name="context">The execution context.</param>
    /// <param name="request">The Principal/get request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The Principal/get response.</returns>
    ValueTask<PrincipalGetResponse> GetAsync(
        JmapExecutionContext context,
        PrincipalGetRequest request,
        CancellationToken cancellationToken = default);
}
