// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using AsyncAPI.Saunter.Generator.Cli.Tests;
using Shouldly;
using Xunit.Abstractions;

namespace AsyncAPI.Saunter.Generator.Build.Tests;

public class IntegrationTests(ITestOutputHelper output)
{
    private string Run(string file, string args, string workingDirectory, int expectedExitCode = 0)
    {
        var process = Process.Start(new ProcessStartInfo(file)
        {
            Arguments = args,
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        });
        process.WaitForExit(TimeSpan.FromSeconds(20));
        var stdOut = process.StandardOutput.ReadToEnd().Trim();
        var stdError = process.StandardError.ReadToEnd().Trim();
        output.WriteLine($"### Output of \"{file} {args}\"");
        output.WriteLine(stdOut);
        output.WriteLine(stdError);

        process.ExitCode.ShouldBe(expectedExitCode);
        return stdOut;
    }

    [Theory]
    [InlineData("StreetlightsAPI")]
    [InlineData("StreetlightsAPI.TopLevelStatement")]
    public void BuildingCsprojGeneratesSpecFilesTest(string csproj)
    {
        var pwd = Directory.GetCurrentDirectory();
        var csprojFullPath = Path.GetFullPath(Path.Combine(pwd, $"../../../../../examples/{csproj}/{csproj}.csproj"));
        output.WriteLine($"Current working directory: {pwd}");
        output.WriteLine($"Csproj under test: {csprojFullPath}");
        File.Exists(csprojFullPath).ShouldBeTrue();

        var csprojDir = Path.GetDirectoryName(csprojFullPath);
        var specDir = Path.Combine(csprojDir, "specs");

        // Spec files should have been generated during the builds of the solution
        Directory.GetFiles(specDir).Length.ShouldBe(2, $"#Spec files initial, path: {specDir}");
        File.ReadAllText(Path.Combine(specDir, "streetlights.yml")).ShouldBe(ExpectedSpecFiles.Yml_v2_6, "yml");
        File.ReadAllText(Path.Combine(specDir, "streetlights.json")).ShouldBe(ExpectedSpecFiles.Json_v2_6, "json");

        // Delete spec files
        foreach (var file in Directory.EnumerateFiles(specDir))
        {
            File.Delete(file);
        }
        Directory.GetFiles(specDir).Length.ShouldBe(0, $"#Spec files after deleting them all, path: {specDir}");

        // Run build
        var stdOut = this.Run("dotnet", "build", csprojDir).Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        stdOut.ShouldContain($"AsyncAPI json successfully written to {Path.Combine(specDir, "streetlights.json")}");
        stdOut.ShouldContain($"AsyncAPI yml successfully written to {Path.Combine(specDir, "streetlights.yml")}");
        stdOut.ShouldContain("Build succeeded.");
        stdOut.ShouldContain("0 Warning(s)");
        stdOut.ShouldContain("0 Error(s)");

        // Check that spec files are actually re-generated
        Directory.GetFiles(specDir).Length.ShouldBe(2, $"#Spec files after running build, path: {specDir}");
        File.ReadAllText(Path.Combine(specDir, "streetlights.yml")).ShouldBe(ExpectedSpecFiles.Yml_v2_6, "yml");
        File.ReadAllText(Path.Combine(specDir, "streetlights.json")).ShouldBe(ExpectedSpecFiles.Json_v2_6, "json");
    }
}
