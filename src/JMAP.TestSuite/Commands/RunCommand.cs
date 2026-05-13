using JMAP.TestSuite.Client;
using JMAP.TestSuite.Rendering;
using JMAP.TestSuite.Runner;
using JMAP.TestSuite.Scenarios.Loading;
using Spectre.Console;
using Spectre.Console.Cli;

namespace JMAP.TestSuite.Commands;

public sealed class RunCommand : AsyncCommand<RunSettings>
{
    protected override async Task<int> ExecuteAsync(
        CommandContext context,
        RunSettings settings,
        CancellationToken cancellationToken)
    {
        var loader = new YamlTestSuiteLoader();
        var runner = new TestRunner();

        var options = new JmapClientOptions
        {
            ApiUrl = new Uri(settings.ApiUrl),
            SessionUrl = string.IsNullOrWhiteSpace(settings.SessionUrl) ? null : new Uri(settings.SessionUrl),
            BearerToken = settings.BearerToken,
            DefaultUsing = settings.DefaultUsing
        };

        try
        {
            var suitePaths = settings.ResolveSuitePaths();
            var results = new List<TestRunResult>();

            foreach (var suitePath in suitePaths)
            {
                var suite = await AnsiConsole.Status()
                    .Spinner(Spinner.Known.Dots)
                    .StartAsync($"Loading [bold]{Markup.Escape(suitePath)}[/]...", _ =>
                        loader.LoadAsync(suitePath, cancellationToken));

                var result = await AnsiConsole.Status()
                    .Spinner(Spinner.Known.Dots)
                    .StartAsync($"Running [bold]{Markup.Escape(suite.Name)}[/]...", _ =>
                        runner.RunSuiteAsync(suite, options, cancellationToken));

                results.Add(result);
                ResultRenderer.Render(result, settings.Verbose, settings.ShowSpec);
            }

            if (results.Count > 1)
            {
                ResultRenderer.RenderOverallSummary(results);
            }

            if (settings.ShowSpec)
            {
                ResultRenderer.RenderCoverageSummary(results);
            }

            return GetExitCode(results);
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
            return 3;
        }
    }

    private static int GetExitCode(IReadOnlyList<TestRunResult> results)
    {
        if (results.Any(x => x.Outcome == TestOutcome.Error))
        {
            return 3;
        }

        if (results.Any(x => x.Outcome == TestOutcome.Canceled))
        {
            return 2;
        }

        if (results.Any(x => x.Outcome == TestOutcome.Failed))
        {
            return 1;
        }

        return 0;
    }
}
