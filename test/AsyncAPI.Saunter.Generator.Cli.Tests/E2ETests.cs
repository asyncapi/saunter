using System.Diagnostics;
using Shouldly;
using Xunit.Abstractions;

namespace AsyncAPI.Saunter.Generator.Cli.Tests;

public class E2ETests(ITestOutputHelper output)
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

    [Fact(Skip = "Manual verification only")]
    public void Pack_Install_Run_Uninstall_Test()
    {
        var workingDirectory = "../../../../../src/AsyncAPI.Saunter.Generator.Cli";
        var stdOut = this.Run("dotnet", "pack", workingDirectory);
        stdOut.ShouldContain("Successfully created package");

        // use --force flag to ensure the test starts clean every run
        stdOut = this.Run("dotnet", "new tool-manifest --force", workingDirectory);
        stdOut.ShouldContain("The template \"Dotnet local tool manifest file\" was created successfully");

        stdOut = this.Run("dotnet", "tool install --local --add-source ./bin/Release AsyncAPI.Saunter.Generator.Cli", workingDirectory);
        stdOut = stdOut.Replace("Skipping NuGet package signature verification.", "").Trim();
        stdOut.ShouldContain("You can invoke the tool from this directory using the following commands: 'dotnet tool run dotnet-asyncapi");
        stdOut.ShouldContain("was successfully installed.");

        stdOut = this.Run("dotnet", "tool list --local asyncapi.saunter.generator.cli", workingDirectory);
        stdOut.ShouldContain("dotnet-asyncapi");

        stdOut = this.Run("dotnet", "tool run dotnet-asyncapi", workingDirectory, 1);
        stdOut.ShouldContain("tofile:  retrieves AsyncAPI from a startup assembly, and writes to file");

        stdOut = this.Run("dotnet", "tool uninstall --local asyncapi.saunter.generator.cli", workingDirectory);
        stdOut.ShouldContain(" was successfully uninstalled");
        stdOut.ShouldContain("removed from manifest file");

        stdOut = this.Run("dotnet", "tool list --local asyncapi.saunter.generator.cli", workingDirectory, 1);
        stdOut.ShouldNotContain("dotnet-asyncapi");
    }
}
