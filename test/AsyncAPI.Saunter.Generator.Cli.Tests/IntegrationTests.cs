using Shouldly;
using Xunit.Abstractions;

namespace AsyncAPI.Saunter.Generator.Cli.Tests;

public class IntegrationTests(ITestOutputHelper output)
{
    private string RunTool(string args, int expectedExitCode = 0)
    {
        using var outWriter = new StringWriter();
        using var errorWriter = new StringWriter();
        Console.SetOut(outWriter);
        Console.SetError(errorWriter);

        var entryPoint = typeof(Program).Assembly.EntryPoint!;
        entryPoint.Invoke(null, new object[] { args.Split(' ') });

        var stdOut = outWriter.ToString();
        var stdError = errorWriter.ToString();
        output.WriteLine($"RUN: {args}");
        output.WriteLine("### STD OUT");
        output.WriteLine(stdOut);
        output.WriteLine("### STD ERROR");
        output.WriteLine(stdError);

        Environment.ExitCode.ShouldBe(expectedExitCode);
        //stdError.ShouldBeEmpty(); LEGO lib doesn't like id: "id is not a valid property at #/components/schemas/lightMeasuredEvent""
        return stdOut;
    }

    [Fact]
    public void DefaultCallPrintsCommandInfo()
    {
        var stdOut = RunTool("tofile").Trim();

        stdOut.ShouldBe("""
                        Usage: tofile [arguments...] [options...] [-h|--help] [--version]
                        
                        Retrieves AsyncAPI spec from a startup assembly and writes to file.
                        
                        Arguments:
                          [0] <string>    relative path to the application's startup assembly
                        
                        Options:
                          -o|--output <string>    relative path where the AsyncAPI will be output [defaults to stdout] (Default: "./")
                          -d|--doc <string>       name(s) of the AsyncAPI documents you want to retrieve as configured in your startup class [defaults to all documents] (Default: null)
                          --format <string>       exports AsyncAPI in json and/or yml format [defaults to json] (Default: "json")
                          --filename <string>     defines the file name template, {document} and {extension} template variables can be used [defaults to "{document}_asyncapi.{extension}\"] (Default: "{document}_asyncapi.{extension}")
                          --env <string>          define environment variable(s) for the application. Formatted as a comma separated list of key=value pairs or just key for flags (Default: "")
                        """, StringCompareShould.IgnoreLineEndings);
    }

    [Fact]
    public void StreetlightsAPIExportSpecTest()
    {
        var path = Directory.GetCurrentDirectory();
        output.WriteLine($"Output path: {path}");
        var stdOut = RunTool($"tofile ../../../../../examples/StreetlightsAPI/bin/Debug/net8.0/StreetlightsAPI.dll --output {path} --format json,yml,yaml");

        stdOut.ShouldNotBeEmpty();
        stdOut.ShouldContain($"AsyncAPI yaml successfully written to {Path.Combine(path, "asyncapi.yaml")}");
        stdOut.ShouldContain($"AsyncAPI yml successfully written to {Path.Combine(path, "asyncapi.yml")}");
        stdOut.ShouldContain($"AsyncAPI json successfully written to {Path.Combine(path, "asyncapi.json")}");

        File.Exists("asyncapi.yml").ShouldBeTrue("asyncapi.yml");
        File.Exists("asyncapi.yaml").ShouldBeTrue("asyncapi.yaml");
        File.Exists("asyncapi.json").ShouldBeTrue("asyncapi.json");

        var yml = File.ReadAllText("asyncapi.yml");
        yml.ShouldBe(ExpectedSpecFiles.Yml_v2_6, "yaml");

        var yaml = File.ReadAllText("asyncapi.yaml");
        yaml.ShouldBe(yml, "yml");

        var json = File.ReadAllText("asyncapi.json");
        json.ShouldBe(ExpectedSpecFiles.Json_v2_6, "json");
    }
}
