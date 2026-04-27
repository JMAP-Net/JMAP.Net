namespace JMAP.Net.Hosting;

/// <summary>
/// Describes how a JMAP method handler may be scheduled by the request dispatcher.
/// </summary>
public enum JmapMethodExecutionMode
{
    /// <summary>
    /// The method must be executed in request order and must not run in parallel with other method calls.
    /// </summary>
    Sequential,

    /// <summary>
    /// The method may run in parallel with other compatible read operations.
    /// </summary>
    ParallelRead,

    /// <summary>
    /// The method mutates state and needs exclusive access to its concurrency key.
    /// </summary>
    ExclusiveWrite
}
