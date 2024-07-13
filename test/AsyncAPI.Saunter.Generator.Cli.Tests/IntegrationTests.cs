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
                          -o|--output <string>    relative path where the AsyncAPI documents will be exported to (Default: "./")
                          -d|--doc <string>       name(s) of the AsyncAPI documents you want to export as configured in your startup class. To export all documents using null. (Default: null)
                          --format <string>       exports AsyncAPI in json and/or yml format (Default: "json")
                          --filename <string>     defines the file name template, {document} and {extension} template variables can be used (Default: "{document}_asyncapi.{extension}")
                          --env <string>          define environment variable(s) for the application. Formatted as a comma separated list of _key=value_ pairs (Default: "")
                        """, StringCompareShould.IgnoreLineEndings);
    }

    /// <remarks>
    /// Both example projects are used to check whether AsyncAPI spec generation is working because they are targeting different .NET versions and are using different hosting strategies.
    /// - StreetlightsAPI project is targeting NET6 using the 'old school' Startup-class hosting mechanism.
    /// - StreetlightsAPI.TopLevelStatement project is targeting NET8 using the new Top Level Statement hosting mechanism.
    /// </remarks>
    [Theory]
    [InlineData("StreetlightsAPI", "net6.0")]
    [InlineData("StreetlightsAPI.TopLevelStatement", "net8.0")]
    public void Streetlights_ExportSpecTest(string csprojName, string targetFramework)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), csprojName);
        output.WriteLine($"Output path: {path}");
        var stdOut = RunTool($"tofile ../../../../../examples/{csprojName}/bin/Debug/{targetFramework}/{csprojName}.dll --output {path} --format json,yml,yaml");

        stdOut.ShouldNotBeEmpty();
        stdOut.ShouldContain($"AsyncAPI yaml successfully written to {Path.Combine(path, "asyncapi.yaml")}");
        stdOut.ShouldContain($"AsyncAPI yml successfully written to {Path.Combine(path, "asyncapi.yml")}");
        stdOut.ShouldContain($"AsyncAPI json successfully written to {Path.Combine(path, "asyncapi.json")}");

        File.Exists(Path.Combine(csprojName, "asyncapi.yml")).ShouldBeTrue("asyncapi.yml");
        File.Exists(Path.Combine(csprojName, "asyncapi.yaml")).ShouldBeTrue("asyncapi.yaml");
        File.Exists(Path.Combine(csprojName, "asyncapi.json")).ShouldBeTrue("asyncapi.json");

        var yml = File.ReadAllText(Path.Combine(csprojName, "asyncapi.yml"));
        yml.ShouldBe(ExpectedSpecFiles.Yml_v2_6, "yml");

        var yaml = File.ReadAllText(Path.Combine(csprojName, "asyncapi.yaml"));
        yaml.ShouldBe(yml, "yaml");

        var json = File.ReadAllText(Path.Combine(csprojName, "asyncapi.json"));
        json.ShouldBe(ExpectedSpecFiles.Json_v2_6, "json");
    }
}
