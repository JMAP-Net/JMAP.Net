using JMAP.TestSuite.Scenarios.Models;

namespace JMAP.TestSuite.Scenarios.Validation;

public sealed class TestSuiteValidator
{
    private static readonly HashSet<string> StepTypes = new(StringComparer.Ordinal)
    {
        "session",
        "jmap",
        "raw"
    };

    private static readonly HashSet<string> AssertionTypes = new(StringComparer.Ordinal)
    {
        "statusCode",
        "jsonPathExists",
        "jsonPathNotExists",
        "jsonPathEquals",
        "jsonPathArrayCount",
        "methodResponse"
    };

    public IReadOnlyList<string> Validate(TestSuiteDefinition suite, bool hasSessionUrl = true)
    {
        var errors = new List<string>();

        RequireText(suite.Id, "Suite id is required.", errors);
        RequireText(suite.Name, "Suite name is required.", errors);

        AddDuplicateErrors(suite.Groups.Select(x => x.Id), "group", errors);

        foreach (var group in suite.Groups)
        {
            RequireText(group.Id, "Group id is required.", errors);
            RequireText(group.Name, $"Group '{group.Id}' name is required.", errors);
            AddDuplicateErrors(group.Scenarios.Select(x => x.Id), $"scenario in group '{group.Id}'", errors);

            foreach (var scenario in group.Scenarios)
            {
                RequireText(scenario.Id, $"Scenario id is required in group '{group.Id}'.", errors);
                RequireText(scenario.Name, $"Scenario '{scenario.Id}' name is required.", errors);
                AddDuplicateErrors(scenario.Steps.Select(x => x.Id), $"step in scenario '{scenario.Id}'", errors);

                foreach (var step in scenario.Steps)
                {
                    ValidateStep(step, scenario.Id, hasSessionUrl, errors);
                }
            }
        }

        return errors;
    }

    private static void ValidateStep(
        TestStepDefinition step,
        string scenarioId,
        bool hasSessionUrl,
        List<string> errors)
    {
        RequireText(step.Id, $"Step id is required in scenario '{scenarioId}'.", errors);

        if (!StepTypes.Contains(step.Type))
        {
            errors.Add($"Step '{step.Id}' has unsupported type '{step.Type}'.");
        }

        if (step.Type == "session" && !hasSessionUrl)
        {
            errors.Add($"Step '{step.Id}' requires a configured session URL.");
        }

        if (step.Type == "jmap" && step.Request is null)
        {
            errors.Add($"Step '{step.Id}' requires a request object.");
        }

        if (step.Type == "jmap" && step.Request is not null && !step.Request.ContainsKey("methodCalls"))
        {
            errors.Add($"Step '{step.Id}' request requires methodCalls.");
        }

        if (step.Type == "raw" && string.IsNullOrWhiteSpace(step.Body))
        {
            errors.Add($"Step '{step.Id}' requires a body.");
        }

        foreach (var assertion in step.Expect)
        {
            ValidateAssertion(step, assertion, errors);
        }
    }

    private static void ValidateAssertion(
        TestStepDefinition step,
        TestAssertionDefinition assertion,
        List<string> errors)
    {
        if (!AssertionTypes.Contains(assertion.Type))
        {
            errors.Add($"Step '{step.Id}' has unsupported assertion '{assertion.Type}'.");
            return;
        }

        if (assertion.Type is "jsonPathExists" or "jsonPathNotExists" or "jsonPathEquals" or "jsonPathArrayCount"
            && string.IsNullOrWhiteSpace(assertion.Path))
        {
            errors.Add($"Assertion '{assertion.Type}' in step '{step.Id}' requires path.");
        }

        if (assertion.Type == "statusCode" && assertion.StatusCode is null)
        {
            errors.Add($"Assertion 'statusCode' in step '{step.Id}' requires a status code.");
        }

        if (assertion.Type == "jsonPathArrayCount" && assertion.Count is null)
        {
            errors.Add($"Assertion 'jsonPathArrayCount' in step '{step.Id}' requires count.");
        }

        if (assertion.Type == "methodResponse"
            && (string.IsNullOrWhiteSpace(assertion.CallId) || string.IsNullOrWhiteSpace(assertion.Name)))
        {
            errors.Add($"Assertion 'methodResponse' in step '{step.Id}' requires callId and name.");
        }
    }

    private static void AddDuplicateErrors(IEnumerable<string> values, string label, List<string> errors)
    {
        foreach (var duplicate in values.Where(x => !string.IsNullOrWhiteSpace(x))
                     .GroupBy(x => x, StringComparer.Ordinal)
                     .Where(x => x.Count() > 1)
                     .Select(x => x.Key))
        {
            errors.Add($"Duplicate {label} id '{duplicate}'.");
        }
    }

    private static void RequireText(string? value, string message, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            errors.Add(message);
        }
    }
}
