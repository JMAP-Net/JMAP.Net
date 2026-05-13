using System.Text.Json.Nodes;
using JMAP.TestSuite.Client;
using JMAP.TestSuite.Client.Auth;
using JMAP.TestSuite.Runner.Assertions;
using JMAP.TestSuite.Scenarios.Models;
using JMAP.TestSuite.Scenarios.Validation;

namespace JMAP.TestSuite.Runner;

public sealed class TestRunner
{
    private readonly TestSuiteValidator _validator = new();
    private readonly IReadOnlyDictionary<string, IAssertionEvaluator> _evaluators;

    public TestRunner()
    {
        _evaluators = new IAssertionEvaluator[]
        {
            new StatusCodeAssertionEvaluator(),
            new JsonPathExistsAssertionEvaluator(),
            new JsonPathNotExistsAssertionEvaluator(),
            new JsonPathEqualsAssertionEvaluator(),
            new JsonPathArrayCountAssertionEvaluator(),
            new MethodResponseAssertionEvaluator()
        }.ToDictionary(x => x.Type, StringComparer.Ordinal);
    }

    public async Task<TestRunResult> RunSuiteAsync(
        TestSuiteDefinition suite,
        JmapClientOptions options,
        CancellationToken cancellationToken)
    {
        return await RunAsync(
            suite,
            options,
            suite.Groups,
            cancellationToken).ConfigureAwait(false);
    }

    public async Task<TestRunResult> RunGroupAsync(
        TestSuiteDefinition suite,
        TestGroupDefinition group,
        JmapClientOptions options,
        CancellationToken cancellationToken)
    {
        return await RunAsync(
            suite,
            options,
            [group],
            cancellationToken).ConfigureAwait(false);
    }

    public async Task<TestRunResult> RunScenarioAsync(
        TestSuiteDefinition suite,
        TestGroupDefinition group,
        TestScenarioDefinition scenario,
        JmapClientOptions options,
        CancellationToken cancellationToken)
    {
        var scopedGroup = new TestGroupDefinition
        {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description,
            Scenarios = [scenario]
        };

        return await RunAsync(
            suite,
            options,
            [scopedGroup],
            cancellationToken).ConfigureAwait(false);
    }

    private async Task<TestRunResult> RunAsync(
        TestSuiteDefinition suite,
        JmapClientOptions options,
        IReadOnlyList<TestGroupDefinition> groups,
        CancellationToken cancellationToken)
    {
        var run = new TestRunResult
        {
            Id = suite.Id,
            Name = suite.Name,
            SpecDocument = suite.Spec?.Document,
            SpecSection = suite.Spec?.Section,
            SpecRequirement = suite.Spec?.Requirement,
            StartedAt = DateTimeOffset.Now,
            Outcome = TestOutcome.Running
        };

        var validationErrors = _validator.Validate(suite, options.SessionUrl is not null);
        if (validationErrors.Count > 0)
        {
            run.Outcome = TestOutcome.Error;
            run.ErrorMessage = string.Join(Environment.NewLine, validationErrors);
            run.FinishedAt = DateTimeOffset.Now;
            return run;
        }

        using var httpClient = new HttpClient();
        var client = new JmapTestClient(httpClient, new BearerTokenAuthProvider(options.BearerToken), options);

        try
        {
            var capabilities = await GetServerCapabilitiesAsync(client, options.SessionUrl is not null, cancellationToken)
                .ConfigureAwait(false);

            if (suite.Skip is not null)
            {
                run.Outcome = TestOutcome.Skipped;
                run.ErrorMessage = suite.Skip.Reason;
                return run;
            }

            var missingSuiteCapabilities = MissingCapabilities(suite.RequiresCapabilities, capabilities).ToList();
            if (missingSuiteCapabilities.Count > 0)
            {
                run.Outcome = TestOutcome.Skipped;
                run.ErrorMessage = $"Missing required capabilities: {string.Join(", ", missingSuiteCapabilities)}";
                return run;
            }

            foreach (var group in groups)
            {
                cancellationToken.ThrowIfCancellationRequested();
                run.Groups.Add(await RunGroupAsync(suite, group, options, client, capabilities, cancellationToken).ConfigureAwait(false));
            }

            run.Outcome = Aggregate(run.Groups.Select(x => x.Outcome));
        }
        catch (OperationCanceledException)
        {
            run.Outcome = TestOutcome.Canceled;
            MarkRunningChildrenCanceled(run);
        }
        finally
        {
            run.FinishedAt = DateTimeOffset.Now;
        }

        return run;
    }

    private async Task<TestGroupResult> RunGroupAsync(
        TestSuiteDefinition suite,
        TestGroupDefinition group,
        JmapClientOptions options,
        JmapTestClient client,
        IReadOnlySet<string> capabilities,
        CancellationToken cancellationToken)
    {
        var result = new TestGroupResult
        {
            Id = group.Id,
            Name = group.Name,
            SpecDocument = group.Spec?.Document ?? suite.Spec?.Document,
            SpecSection = group.Spec?.Section ?? suite.Spec?.Section,
            SpecRequirement = group.Spec?.Requirement ?? suite.Spec?.Requirement,
            StartedAt = DateTimeOffset.Now,
            Outcome = TestOutcome.Running
        };

        try
        {
            if (group.Skip is not null)
            {
                result.Outcome = TestOutcome.Skipped;
                result.ErrorMessage = group.Skip.Reason;
                return result;
            }

            var missingCapabilities = MissingCapabilities(group.RequiresCapabilities, capabilities).ToList();
            if (missingCapabilities.Count > 0)
            {
                result.Outcome = TestOutcome.Skipped;
                result.ErrorMessage = $"Missing required capabilities: {string.Join(", ", missingCapabilities)}";
                return result;
            }

            foreach (var scenario in group.Scenarios)
            {
                cancellationToken.ThrowIfCancellationRequested();
                result.Scenarios.Add(await RunScenarioAsync(suite, scenario, options, client, capabilities, cancellationToken).ConfigureAwait(false));
            }

            result.Outcome = Aggregate(result.Scenarios.Select(x => x.Outcome));
        }
        catch (OperationCanceledException)
        {
            result.Outcome = TestOutcome.Canceled;
            throw;
        }
        finally
        {
            result.FinishedAt = DateTimeOffset.Now;
        }

        return result;
    }

    private async Task<TestScenarioResult> RunScenarioAsync(
        TestSuiteDefinition suite,
        TestScenarioDefinition scenario,
        JmapClientOptions options,
        JmapTestClient client,
        IReadOnlySet<string> capabilities,
        CancellationToken cancellationToken)
    {
        var result = new TestScenarioResult
        {
            Id = scenario.Id,
            Name = scenario.Name,
            SpecDocument = scenario.Spec?.Document ?? suite.Spec?.Document,
            SpecSection = scenario.Spec?.Section,
            SpecRequirement = scenario.Spec?.Requirement,
            StartedAt = DateTimeOffset.Now,
            Outcome = TestOutcome.Running
        };

        try
        {
            if (scenario.Skip is not null)
            {
                result.Outcome = TestOutcome.Skipped;
                result.ErrorMessage = scenario.Skip.Reason;
                return result;
            }

            var missingCapabilities = MissingCapabilities(scenario.RequiresCapabilities, capabilities).ToList();
            if (missingCapabilities.Count > 0)
            {
                result.Outcome = TestOutcome.Skipped;
                result.ErrorMessage = $"Missing required capabilities: {string.Join(", ", missingCapabilities)}";
                return result;
            }

            foreach (var step in scenario.Steps)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var stepResult = await RunStepAsync(suite, step, options, client, cancellationToken).ConfigureAwait(false);
                result.Steps.Add(stepResult);

                if (stepResult.Outcome is TestOutcome.Failed or TestOutcome.Error)
                {
                    break;
                }
            }

            result.Outcome = Aggregate(result.Steps.Select(x => x.Outcome));
        }
        catch (OperationCanceledException)
        {
            result.Outcome = TestOutcome.Canceled;
            throw;
        }
        finally
        {
            result.FinishedAt = DateTimeOffset.Now;
        }

        return result;
    }

    private async Task<TestStepResult> RunStepAsync(
        TestSuiteDefinition suite,
        TestStepDefinition step,
        JmapClientOptions options,
        JmapTestClient client,
        CancellationToken cancellationToken)
    {
        var result = new TestStepResult
        {
            Id = step.Id,
            Name = step.Id,
            StartedAt = DateTimeOffset.Now,
            Outcome = TestOutcome.Running
        };

        try
        {
            var httpResult = step.Type switch
            {
                "session" => await client.GetSessionAsync(cancellationToken).ConfigureAwait(false),
                "jmap" => await client.SendAsync(BuildRequest(suite, step, options), cancellationToken).ConfigureAwait(false),
                "raw" => await client.SendRawAsync(step.Body!, cancellationToken).ConfigureAwait(false),
                _ => throw new InvalidOperationException($"Unsupported step type '{step.Type}'.")
            };

            result.HttpResult = httpResult;

            if (httpResult.JsonParseError is not null && step.Expect.Any(IsJsonAssertion))
            {
                result.Assertions.Add(new AssertionResult
                {
                    Type = "json",
                    Outcome = TestOutcome.Failed,
                    Message = $"Response body is not valid JSON: {httpResult.JsonParseError}"
                });
            }

            foreach (var assertion in step.Expect)
            {
                if (_evaluators.TryGetValue(assertion.Type, out var evaluator))
                {
                    result.Assertions.Add(evaluator.Evaluate(assertion, httpResult));
                }
                else
                {
                    result.Assertions.Add(new AssertionResult
                    {
                        Type = assertion.Type,
                        Outcome = TestOutcome.Error,
                        Message = $"Unsupported assertion '{assertion.Type}'."
                    });
                }
            }

            result.Outcome = Aggregate(result.Assertions.Select(x => x.Outcome));
        }
        catch (OperationCanceledException)
        {
            result.Outcome = TestOutcome.Canceled;
            throw;
        }
        catch (Exception ex)
        {
            result.Outcome = TestOutcome.Error;
            result.ErrorMessage = ex.Message;
        }
        finally
        {
            result.FinishedAt = DateTimeOffset.Now;
        }

        return result;
    }

    private static JsonObject BuildRequest(
        TestSuiteDefinition suite,
        TestStepDefinition step,
        JmapClientOptions options)
    {
        var request = step.Request?.DeepClone().AsObject()
            ?? throw new InvalidOperationException($"Step '{step.Id}' requires a request.");

        if (!request.ContainsKey("using"))
        {
            var usingValues = suite.Defaults.Using.Count > 0 ? suite.Defaults.Using : options.DefaultUsing;
            request["using"] = new JsonArray(usingValues.Select(x => JsonValue.Create(x)).ToArray());
        }

        return request;
    }

    private static bool IsJsonAssertion(TestAssertionDefinition assertion)
        => assertion.Type.StartsWith("jsonPath", StringComparison.Ordinal)
           || assertion.Type == "methodResponse";

    private static TestOutcome Aggregate(IEnumerable<TestOutcome> outcomes)
    {
        var list = outcomes.ToList();
        if (list.Count == 0)
        {
            return TestOutcome.Passed;
        }

        if (list.Contains(TestOutcome.Error))
        {
            return TestOutcome.Error;
        }

        if (list.Contains(TestOutcome.Canceled))
        {
            return TestOutcome.Canceled;
        }

        if (list.All(x => x == TestOutcome.Skipped))
        {
            return TestOutcome.Skipped;
        }

        if (list.Contains(TestOutcome.Failed))
        {
            return TestOutcome.Failed;
        }

        if (list.Contains(TestOutcome.Skipped))
        {
            return TestOutcome.Passed;
        }

        return list.All(x => x == TestOutcome.Passed) ? TestOutcome.Passed : TestOutcome.NotRun;
    }

    private static async Task<IReadOnlySet<string>> GetServerCapabilitiesAsync(
        JmapTestClient client,
        bool hasSessionUrl,
        CancellationToken cancellationToken)
    {
        if (!hasSessionUrl)
        {
            return new HashSet<string>(StringComparer.Ordinal);
        }

        try
        {
            var session = await client.GetSessionAsync(cancellationToken).ConfigureAwait(false);
            if (session.ResponseJson is not JsonObject root
                || root["capabilities"] is not JsonObject capabilities)
            {
                return new HashSet<string>(StringComparer.Ordinal);
            }

            return capabilities.Select(x => x.Key).ToHashSet(StringComparer.Ordinal);
        }
        catch
        {
            return new HashSet<string>(StringComparer.Ordinal);
        }
    }

    private static IEnumerable<string> MissingCapabilities(
        IEnumerable<string> required,
        IReadOnlySet<string> available)
        => required.Where(capability => !available.Contains(capability));

    private static void MarkRunningChildrenCanceled(TestRunResult run)
    {
        foreach (var group in run.Groups.Where(x => x.Outcome == TestOutcome.Running))
        {
            group.Outcome = TestOutcome.Canceled;

            foreach (var scenario in group.Scenarios.Where(x => x.Outcome == TestOutcome.Running))
            {
                scenario.Outcome = TestOutcome.Canceled;

                foreach (var step in scenario.Steps.Where(x => x.Outcome == TestOutcome.Running))
                {
                    step.Outcome = TestOutcome.Canceled;
                }
            }
        }
    }
}
