// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using Shouldly;
using Xunit.Abstractions;

namespace AsyncAPI.Saunter.Generator.Cli.Tests;

public class PackAndInstallLocalTests(ITestOutputHelper output)
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

    [Fact]
    public void Pack_Install_Run_Uninstall_Test()
    {
        var stdOut = this.Run("dotnet", "pack", "../../../../../src/AsyncAPI.Saunter.Generator.Cli");
        stdOut.ShouldContain("Successfully created package");

        stdOut = this.Run("dotnet", "tool install --global --add-source ./bin/Release AsyncAPI.Saunter.Generator.Cli", "../../../../../src/AsyncAPI.Saunter.Generator.Cli");
        stdOut.ShouldBeOneOf("You can invoke the tool using the following command: AsyncAPI.NET\r\nTool 'asyncapi.saunter.generator.cli' (version '1.0.0') was successfully installed.",
                             "Tool 'asyncapi.saunter.generator.cli' was reinstalled with the stable version (version '1.0.0').");

        stdOut = this.Run("dotnet", "tool list -g asyncapi.saunter.generator.cli", "");
        stdOut.ShouldContain("AsyncAPI.NET");

        stdOut = this.Run("asyncapi.cmd", "", "", 1);
        stdOut.ShouldContain("tofile:  retrieves AsyncAPI from a startup assembly, and writes to file");

        stdOut = this.Run("dotnet", "tool uninstall -g asyncapi.saunter.generator.cli", "");
        stdOut.ShouldContain(" was successfully uninstalled.");

        stdOut = this.Run("dotnet", "tool list -g asyncapi.saunter.generator.cli", "", 1);
        stdOut.ShouldNotContain("AsyncAPI.NET");
    }
}
