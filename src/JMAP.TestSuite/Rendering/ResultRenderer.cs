using JMAP.TestSuite.Runner;
using Spectre.Console;

namespace JMAP.TestSuite.Rendering;

public static class ResultRenderer
{
    public static void Render(TestRunResult result, bool verbose, bool showSpec)
    {
        AnsiConsole.Write(new Rule($"[bold]{Markup.Escape(result.Name)}[/]").RuleStyle("grey"));
        RenderSummary(result);
        RenderTree(result, showSpec);
        RenderFailures(result, verbose);
    }

    public static void RenderOverallSummary(IReadOnlyList<TestRunResult> results)
    {
        AnsiConsole.Write(new Rule("[bold]Overall Summary[/]").RuleStyle("grey"));

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("Suite")
            .AddColumn("Outcome")
            .AddColumn(new TableColumn("Passed").RightAligned())
            .AddColumn(new TableColumn("Failed").RightAligned())
            .AddColumn(new TableColumn("Errors").RightAligned())
            .AddColumn(new TableColumn("Skipped").RightAligned())
            .AddColumn(new TableColumn("Duration").RightAligned());

        foreach (var result in results)
        {
            var assertions = FlattenAssertions(result).ToList();
            table.AddRow(
                Markup.Escape(result.Name),
                FormatOutcome(result.Outcome),
                assertions.Count(x => x.Outcome == TestOutcome.Passed).ToString(),
                assertions.Count(x => x.Outcome == TestOutcome.Failed).ToString(),
                assertions.Count(x => x.Outcome == TestOutcome.Error).ToString(),
                assertions.Count(x => x.Outcome == TestOutcome.Skipped).ToString(),
                $"{result.Duration.TotalMilliseconds:N0} ms");
        }

        table.AddRow(
            "[bold]Total[/]",
            FormatOutcome(Aggregate(results.Select(x => x.Outcome))),
            results.SelectMany(FlattenAssertions).Count(x => x.Outcome == TestOutcome.Passed).ToString(),
            results.SelectMany(FlattenAssertions).Count(x => x.Outcome == TestOutcome.Failed).ToString(),
            results.SelectMany(FlattenAssertions).Count(x => x.Outcome == TestOutcome.Error).ToString(),
            results.SelectMany(FlattenAssertions).Count(x => x.Outcome == TestOutcome.Skipped).ToString(),
            $"{results.Aggregate(TimeSpan.Zero, (sum, result) => sum + result.Duration).TotalMilliseconds:N0} ms");

        AnsiConsole.Write(table);
    }

    private static void RenderSummary(TestRunResult result)
    {
        var counts = FlattenAssertions(result)
            .GroupBy(x => x.Outcome)
            .ToDictionary(x => x.Key, x => x.Count());

        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("Summary")
            .AddColumn("Outcome")
            .AddColumn(new TableColumn("Count").RightAligned());

        table.AddRow("[green]Passed[/]", GetCount(counts, TestOutcome.Passed).ToString());
        table.AddRow("[red]Failed[/]", GetCount(counts, TestOutcome.Failed).ToString());
        table.AddRow("[yellow]Errors[/]", GetCount(counts, TestOutcome.Error).ToString());
        table.AddRow("[grey]Canceled[/]", GetCount(counts, TestOutcome.Canceled).ToString());
        table.AddRow("[grey]Skipped[/]", GetCount(counts, TestOutcome.Skipped).ToString());
        table.AddRow("Duration", $"{result.Duration.TotalMilliseconds:N0} ms");
        table.AddRow("Run outcome", FormatOutcome(result.Outcome));

        AnsiConsole.Write(table);
    }

    private static void RenderTree(TestRunResult result, bool showSpec)
    {
        var tree = new Tree($"{FormatOutcome(result.Outcome)} {Markup.Escape(result.Name)}{FormatSpec(result, showSpec)}");

        foreach (var group in result.Groups)
        {
            var groupNode = tree.AddNode($"{FormatOutcome(group.Outcome)} {Markup.Escape(group.Name)}{FormatSpec(group, showSpec)}");
            foreach (var scenario in group.Scenarios)
            {
                var scenarioNode = groupNode.AddNode($"{FormatOutcome(scenario.Outcome)} {Markup.Escape(scenario.Name)}{FormatSpec(scenario, showSpec)}");
                foreach (var step in scenario.Steps)
                {
                    var stepNode = scenarioNode.AddNode($"{FormatOutcome(step.Outcome)} {Markup.Escape(step.Name)}");
                    foreach (var assertion in step.Assertions)
                    {
                        stepNode.AddNode($"{FormatOutcome(assertion.Outcome)} {Markup.Escape(assertion.Type)} - {Markup.Escape(assertion.Message)}");
                    }
                }
            }
        }

        AnsiConsole.Write(tree);
    }

    private static void RenderFailures(TestRunResult result, bool verbose)
    {
        if (result.Outcome == TestOutcome.Skipped)
        {
            AnsiConsole.MarkupLine($"[grey]Skipped: {Markup.Escape(result.ErrorMessage ?? "No reason provided.")}[/]");
            return;
        }

        var failedSteps = result.Groups
            .SelectMany(group => group.Scenarios.Select(scenario => new { group, scenario }))
            .SelectMany(x => x.scenario.Steps.Select(step => new { x.group, x.scenario, step }))
            .Where(x => x.step.Outcome is TestOutcome.Failed or TestOutcome.Error)
            .ToList();

        if (failedSteps.Count == 0)
        {
            AnsiConsole.MarkupLine("[green]All checks passed.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("Failures")
            .AddColumn("Group")
            .AddColumn("Scenario")
            .AddColumn("Step")
            .AddColumn("Message");

        foreach (var item in failedSteps)
        {
            var message = item.step.ErrorMessage
                          ?? string.Join(Environment.NewLine,
                              item.step.Assertions
                                  .Where(x => x.Outcome is TestOutcome.Failed or TestOutcome.Error)
                                  .Select(x => x.Message));

            table.AddRow(
                Markup.Escape(item.group.Name),
                Markup.Escape(item.scenario.Name),
                Markup.Escape(item.step.Name),
                Markup.Escape(message));
        }

        AnsiConsole.Write(table);

        if (!verbose)
        {
            return;
        }

        foreach (var item in failedSteps.Where(x => x.step.HttpResult is not null))
        {
            AnsiConsole.Write(new Rule(Markup.Escape($"{item.scenario.Name} / {item.step.Name}")).RuleStyle("red"));
            AnsiConsole.Write(new Panel(item.step.HttpResult!.RequestBody ?? "")
                .Header("Request")
                .Border(BoxBorder.Rounded));
            AnsiConsole.Write(new Panel(item.step.HttpResult.ResponseBody)
                .Header($"Response HTTP {item.step.HttpResult.StatusCode}")
                .Border(BoxBorder.Rounded));
        }
    }

    private static IEnumerable<AssertionResult> FlattenAssertions(TestRunResult result)
        => result.Groups
            .SelectMany(group => group.Scenarios)
            .SelectMany(scenario => scenario.Steps)
            .SelectMany(step => step.Assertions);

    public static void RenderCoverageSummary(IReadOnlyList<TestRunResult> results)
    {
        var scenarios = results
            .SelectMany(run => run.Groups)
            .SelectMany(group => group.Scenarios)
            .Where(scenario => !string.IsNullOrWhiteSpace(scenario.SpecDocument))
            .GroupBy(
                scenario => new
                {
                    Document = scenario.SpecDocument!,
                    Section = scenario.SpecSection ?? ""
                })
            .OrderBy(group => group.Key.Document, StringComparer.Ordinal)
            .ThenBy(group => group.Key.Section, StringComparer.Ordinal)
            .ToList();

        if (scenarios.Count == 0)
        {
            return;
        }

        AnsiConsole.Write(new Rule("[bold]Spec Coverage[/]").RuleStyle("grey"));

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("Document")
            .AddColumn("Section")
            .AddColumn(new TableColumn("Scenarios").RightAligned())
            .AddColumn(new TableColumn("Passed").RightAligned())
            .AddColumn(new TableColumn("Failed").RightAligned())
            .AddColumn(new TableColumn("Skipped").RightAligned());

        foreach (var group in scenarios)
        {
            var items = group.ToList();
            table.AddRow(
                Markup.Escape(group.Key.Document),
                Markup.Escape(string.IsNullOrWhiteSpace(group.Key.Section) ? "-" : group.Key.Section),
                items.Count.ToString(),
                items.Count(x => x.Outcome == TestOutcome.Passed).ToString(),
                items.Count(x => x.Outcome == TestOutcome.Failed || x.Outcome == TestOutcome.Error).ToString(),
                items.Count(x => x.Outcome == TestOutcome.Skipped).ToString());
        }

        AnsiConsole.Write(table);
    }

    private static int GetCount(IReadOnlyDictionary<TestOutcome, int> counts, TestOutcome outcome)
        => counts.TryGetValue(outcome, out var count) ? count : 0;

    private static string FormatOutcome(TestOutcome outcome)
        => outcome switch
        {
            TestOutcome.Passed => "[green]PASS[/]",
            TestOutcome.Failed => "[red]FAIL[/]",
            TestOutcome.Error => "[yellow]ERROR[/]",
            TestOutcome.Canceled => "[grey]CANCELED[/]",
            TestOutcome.Skipped => "[grey]SKIP[/]",
            TestOutcome.Running => "[blue]RUNNING[/]",
            _ => "[grey]NOT RUN[/]"
        };

    private static string FormatSpec(TestRunResult result, bool showSpec)
        => showSpec ? FormatSpec(result.SpecDocument, result.SpecSection, result.SpecRequirement) : "";

    private static string FormatSpec(TestGroupResult result, bool showSpec)
        => showSpec ? FormatSpec(result.SpecDocument, result.SpecSection, result.SpecRequirement) : "";

    private static string FormatSpec(TestScenarioResult result, bool showSpec)
        => showSpec ? FormatSpec(result.SpecDocument, result.SpecSection, result.SpecRequirement) : "";

    private static string FormatSpec(string? document, string? section, string? requirement)
    {
        if (string.IsNullOrWhiteSpace(document))
        {
            return "";
        }

        var reference = string.IsNullOrWhiteSpace(section)
            ? document
            : $"{document} section {section}";

        return string.IsNullOrWhiteSpace(requirement)
            ? $" [grey]({Markup.Escape(reference)})[/]"
            : $" [grey]({Markup.Escape(reference)} - {Markup.Escape(requirement)})[/]";
    }

    private static TestOutcome Aggregate(IEnumerable<TestOutcome> outcomes)
    {
        var list = outcomes.ToList();
        if (list.Contains(TestOutcome.Error))
        {
            return TestOutcome.Error;
        }

        if (list.Contains(TestOutcome.Canceled))
        {
            return TestOutcome.Canceled;
        }

        if (list.Contains(TestOutcome.Failed))
        {
            return TestOutcome.Failed;
        }

        if (list.All(x => x == TestOutcome.Skipped))
        {
            return TestOutcome.Skipped;
        }

        return TestOutcome.Passed;
    }
}
