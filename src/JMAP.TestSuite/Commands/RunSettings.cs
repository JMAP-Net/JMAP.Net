using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace JMAP.TestSuite.Commands;

public sealed class RunSettings : CommandSettings
{
    [CommandArgument(0, "<suites>")]
    [Description("Path, paths, or glob patterns for YAML suite files.")]
    public string[] SuitePaths { get; init; } = [];

    [CommandOption("--api-url <URL>")]
    [Description("JMAP API endpoint URL.")]
    public string ApiUrl { get; init; } = "";

    [CommandOption("--session-url <URL>")]
    [Description("Optional JMAP session endpoint URL.")]
    public string? SessionUrl { get; init; }

    [CommandOption("--token <TOKEN>")]
    [Description("Bearer token used as Authorization header.")]
    public string BearerToken { get; init; } = "";

    [CommandOption("--using <URI>")]
    [Description("Default JMAP capability URI. Can be repeated.")]
    public string[] DefaultUsing { get; init; } = [];

    [CommandOption("--verbose")]
    [Description("Render request and response bodies for failed steps.")]
    public bool Verbose { get; init; }

    [CommandOption("--show-spec")]
    [Description("Render spec references and a coverage summary.")]
    public bool ShowSpec { get; init; }

    public override ValidationResult Validate()
    {
        if (SuitePaths.Length == 0 || SuitePaths.Any(string.IsNullOrWhiteSpace))
        {
            return ValidationResult.Error("At least one suite path or glob pattern is required.");
        }

        if (!Uri.TryCreate(ApiUrl, UriKind.Absolute, out _))
        {
            return ValidationResult.Error("--api-url must be an absolute URL.");
        }

        if (!string.IsNullOrWhiteSpace(SessionUrl) && !Uri.TryCreate(SessionUrl, UriKind.Absolute, out _))
        {
            return ValidationResult.Error("--session-url must be an absolute URL when provided.");
        }

        if (string.IsNullOrWhiteSpace(BearerToken))
        {
            return ValidationResult.Error("--token is required.");
        }

        return ValidationResult.Success();
    }

    public IReadOnlyList<string> ResolveSuitePaths()
    {
        var resolved = new List<string>();
        var missing = new List<string>();

        foreach (var suitePath in SuitePaths)
        {
            var matches = ResolveSuitePath(suitePath).ToList();
            if (matches.Count == 0)
            {
                missing.Add(CreateMissingSuiteMessage(suitePath));
                continue;
            }

            resolved.AddRange(matches);
        }

        if (missing.Count > 0)
        {
            throw new FileNotFoundException(string.Join(Environment.NewLine + Environment.NewLine, missing));
        }

        return resolved.Distinct(StringComparer.OrdinalIgnoreCase).Order(StringComparer.OrdinalIgnoreCase).ToList();
    }

    private static IEnumerable<string> ResolveSuitePath(string suitePath)
    {
        if (!HasGlobPattern(suitePath))
        {
            return File.Exists(suitePath) ? [suitePath] : [];
        }

        var directoryPart = Path.GetDirectoryName(suitePath);
        var pattern = Path.GetFileName(suitePath);
        var directory = string.IsNullOrWhiteSpace(directoryPart) ? "." : directoryPart;

        return Directory.Exists(directory)
            ? Directory.EnumerateFiles(directory, pattern, SearchOption.TopDirectoryOnly)
            : [];
    }

    private static bool HasGlobPattern(string value)
        => value.Contains('*', StringComparison.Ordinal) || value.Contains('?', StringComparison.Ordinal);

    private static string CreateMissingSuiteMessage(string suitePath)
    {
        var message = HasGlobPattern(suitePath)
            ? $"Suite glob did not match any files: {suitePath}"
            : $"Suite file does not exist: {suitePath}";
        var fileName = Path.GetFileName(suitePath);
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return message;
        }

        var baseDirectory = AppContext.BaseDirectory;
        var repositoryRoot = FindRepositoryRoot(baseDirectory);
        if (repositoryRoot is null)
        {
            return message;
        }

        var suitesDirectory = Path.Combine(repositoryRoot, "src", "JMAP.TestSuite", "Suites");
        if (!Directory.Exists(suitesDirectory))
        {
            return message;
        }

        var suggestions = Directory
            .EnumerateFiles(suitesDirectory, fileName, SearchOption.AllDirectories)
            .Select(path => Path.GetRelativePath(repositoryRoot, path))
            .Take(5)
            .ToList();

        return suggestions.Count == 0
            ? message
            : $"{message}{Environment.NewLine}Did you mean:{Environment.NewLine}  {string.Join($"{Environment.NewLine}  ", suggestions)}";
    }

    private static string? FindRepositoryRoot(string startDirectory)
    {
        var directory = new DirectoryInfo(startDirectory);
        while (directory is not null)
        {
            if (Directory.Exists(Path.Combine(directory.FullName, ".git"))
                || File.Exists(Path.Combine(directory.FullName, "src", "JMAP.Net.slnx")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        return null;
    }
}
