namespace JMAP.Net.Hosting;

/// <summary>
/// Represents a handled JMAP method-level error.
/// </summary>
public sealed class JmapMethodException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JmapMethodException" /> class.
    /// </summary>
    /// <param name="errorType">The JMAP method error type.</param>
    /// <param name="message">The error description.</param>
    public JmapMethodException(string errorType, string message)
        : base(message)
    {
        ErrorType = errorType;
    }

    /// <summary>
    /// Gets the JMAP method error type.
    /// </summary>
    public string ErrorType { get; }
}
