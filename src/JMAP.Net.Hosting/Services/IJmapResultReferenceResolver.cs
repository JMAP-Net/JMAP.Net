using JMAP.Net.Common.Protocol;

namespace JMAP.Net.Hosting.Services;

internal interface IJmapResultReferenceResolver
{
    bool ContainsResultReference(Invocation invocation);

    bool TryResolve(
        Invocation invocation,
        IReadOnlyList<Invocation?> previousResponses,
        out Invocation resolvedInvocation);
}
