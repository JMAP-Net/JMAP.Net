using JMAP.Net.Capabilities.Core;
using JMAP.Net.Common.Protocol;

namespace JMAP.Net.Tests.Hosting.Infrastructure;

internal static class JmapRequestBuilder
{
    public static JmapRequest CoreRequest(params Invocation[] invocations)
    {
        return new JmapRequest
        {
            Using = [CoreCapability.CapabilityUri],
            MethodCalls = invocations.ToList(),
            CreatedIds = null
        };
    }

    public static JmapRequest UnsupportedCapabilityRequest(params Invocation[] invocations)
    {
        return new JmapRequest
        {
            Using = ["urn:example:unsupported"],
            MethodCalls = invocations.ToList(),
            CreatedIds = null
        };
    }

    public static Invocation CoreEcho(string callId, string key = "hello", string value = "world")
    {
        return Invocation(
            "Core/echo",
            callId,
            new Dictionary<string, object?> { [key] = value });
    }

    public static Invocation DelayedRead(string callId, int delayMilliseconds)
    {
        return Invocation(
            "Test/read",
            callId,
            new Dictionary<string, object?>
            {
                ["delayMilliseconds"] = delayMilliseconds
            });
    }

    public static Invocation ExclusiveWrite(string callId, int delayMilliseconds, string accountId)
    {
        return Invocation(
            "Test/write",
            callId,
            new Dictionary<string, object?>
            {
                ["delayMilliseconds"] = delayMilliseconds,
                ["accountId"] = accountId
            });
    }

    public static Invocation Invocation(
        string name,
        string callId,
        Dictionary<string, object?>? arguments = null)
    {
        return new Invocation
        {
            Name = name,
            Arguments = arguments ?? new Dictionary<string, object?>(),
            MethodCallId = callId
        };
    }
}
