// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using Shouldly;
using Xunit.Abstractions;

namespace AsyncAPI.Saunter.Generator.Cli.Tests;

public class DotnetCliToolTests(ITestOutputHelper output)
{
    private string RunTool(string args, int expectedExitCode = 0)
    {
        var process = Process.Start(new ProcessStartInfo("dotnet")
        {
            Arguments = $"../../../../../src/AsyncAPI.Saunter.Generator.Cli/bin/Debug/net6.0/AsyncAPI.Saunter.Generator.Cli.dll tofile {args}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        });
        process.WaitForExit();
        var stdOut = process.StandardOutput.ReadToEnd().Trim();
        var stdError = process.StandardError.ReadToEnd().Trim();
        output.WriteLine(stdOut);
        output.WriteLine(stdError);

        process.ExitCode.ShouldBe(expectedExitCode);
        //stdError.ShouldBeEmpty(); LEGO lib doesn't like id: "id is not a valid property at #/components/schemas/lightMeasuredEvent""
        return stdOut;
    }

    [Fact]
    public void DefaultCallPrintsCommandInfo()
    {
        var stdOut = RunTool("", 1);

        stdOut.ShouldBe("""
                        Usage: dotnet asyncapi tofile [options] [startupassembly]

                        startupassembly:
                          relative path to the application's startup assembly

                        options:
                          --doc:  name(s) of the AsyncAPI documents you want to retrieve, as configured in your startup class [defaults to all documents]
                          --output:  relative path where the AsyncAPI will be output [defaults to stdout]
                          --filename:  defines the file name template, {document} and {extension} template variables can be used [defaults to "{document}_asyncapi.{extension}"]
                          --format:  exports AsyncAPI in json and/or yml format [defaults to json]
                          --env:  define environment variable(s) for the application during generation of the AsyncAPI files [defaults to empty, can be used to define for example ASPNETCORE_ENVIRONMENT]
                        """, StringCompareShould.IgnoreLineEndings);
    }

    [Fact]
    public void StreetlightsAPIExportSpecTest()
    {
        var path = Directory.GetCurrentDirectory();
        output.WriteLine($"Output path: {path}");
        var stdOut = RunTool($"--output {path} --format json,yml,yaml ../../../../../examples/StreetlightsAPI/bin/Debug/net6.0/StreetlightsAPI.dll");

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
