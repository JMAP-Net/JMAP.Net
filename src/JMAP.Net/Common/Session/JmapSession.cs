using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Common.Session;

/// <summary>
/// Represents a JMAP Session resource that provides details about the data and capabilities
/// the server can provide to the client.
/// As per RFC 8620, Section 2.
/// </summary>
public class JmapSession
{
    /// <summary>
    /// An object specifying the capabilities of this server.
    /// Each key is a URI for a capability supported by the server.
    /// </summary>
    [JsonPropertyName("capabilities")]
    public required Dictionary<string, object> Capabilities { get; init; }

    /// <summary>
    /// A map of account id to Account object for each account the user has access to.
    /// </summary>
    [JsonPropertyName("accounts")]
    public required Dictionary<JmapId, JmapAccount> Accounts { get; init; }

    /// <summary>
    /// A map of capability URIs to the account id that is the user's main/default account
    /// for that capability.
    /// </summary>
    [JsonPropertyName("primaryAccounts")]
    public required Dictionary<string, JmapId> PrimaryAccounts { get; init; }

    /// <summary>
    /// The username associated with the given credentials, or empty string if none.
    /// </summary>
    [JsonPropertyName("username")]
    public required string Username { get; init; }

    /// <summary>
    /// The URL to use for JMAP API requests.
    /// </summary>
    [JsonPropertyName("apiUrl")]
    public required string ApiUrl { get; init; }

    /// <summary>
    /// The URL endpoint to use when downloading files (in URI Template format).
    /// Must contain variables: accountId, blobId, type, and name.
    /// </summary>
    [JsonPropertyName("downloadUrl")]
    public required string DownloadUrl { get; init; }

    /// <summary>
    /// The URL endpoint to use when uploading files (in URI Template format).
    /// Must contain variable: accountId.
    /// </summary>
    [JsonPropertyName("uploadUrl")]
    public required string UploadUrl { get; init; }

    /// <summary>
    /// The URL to connect to for push events (in URI Template format).
    /// Must contain variables: types, closeafter, and ping.
    /// </summary>
    [JsonPropertyName("eventSourceUrl")]
    public required string EventSourceUrl { get; init; }

    /// <summary>
    /// A string representing the state of this session object.
    /// Changes when any property changes.
    /// </summary>
    [JsonPropertyName("state")]
    public required string State { get; init; }
}
