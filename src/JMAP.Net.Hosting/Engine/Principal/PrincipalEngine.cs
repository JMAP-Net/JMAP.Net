using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Capabilities.Sharing.Methods.Principal;
using JMAP.Net.Common.Errors;
using JMAP.Net.Persistence.Sharing;

namespace JMAP.Net.Hosting.Engine.Principal;

/// <summary>
/// Default implementation of Principal method semantics.
/// </summary>
public sealed class PrincipalEngine(IPrincipalStore store) : IPrincipalEngine
{
    /// <inheritdoc />
    public async ValueTask<PrincipalGetResponse> GetAsync(
        JmapExecutionContext context,
        PrincipalGetRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);

        if (!context.AccountIds.Contains(request.AccountId))
        {
            throw new JmapMethodException(
                JmapErrorType.AccountNotFound,
                $"The account '{request.AccountId}' was not found.");
        }

        var state = await store.GetStateAsync(request.AccountId, cancellationToken);
        if (state is null)
        {
            throw new JmapMethodException(
                JmapErrorType.AccountNotFound,
                $"The account '{request.AccountId}' was not found.");
        }

        var principals = await store.GetAsync(request.AccountId, request.Ids, cancellationToken);
        var list = principals.ToList();
        var foundIds = list.Select(principal => principal.Id).ToHashSet();
        var notFound = request.Ids is null
            ? []
            : request.Ids.Where(id => !foundIds.Contains(id)).ToList();

        return new PrincipalGetResponse
        {
            AccountId = request.AccountId,
            State = state,
            List = list,
            NotFound = notFound
        };
    }
}
