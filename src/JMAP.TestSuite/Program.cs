using JMAP.TestSuite.Commands;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.SetApplicationName("jmap-testsuite");
    config.SetApplicationVersion("0.1.0");

    config.AddCommand<RunCommand>("run")
        .WithDescription("Runs a YAML JMAP test suite against an external JMAP server.")
        .WithExample(
            "run",
            "Suites/compliance/*.yaml",
            "--api-url",
            "https://example.com/jmap",
            "--session-url",
            "https://example.com/.well-known/jmap",
            "--token",
            "secret");
});

return await app.RunAsync(args);
