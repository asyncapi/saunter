// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Reflection;
using static Program;

namespace AsyncApi.Saunter.Generator.Cli.Commands;

internal class Tofile
{
    internal static Func<IDictionary<string, string>, int> Run(string[] args) => namedArgs =>
    {
        if (!File.Exists(namedArgs[StartupAssemblyArgument]))
        {
            throw new FileNotFoundException(namedArgs[StartupAssemblyArgument]);
        }

        var depsFile = namedArgs[StartupAssemblyArgument].Replace(".dll", ".deps.json");
        var runtimeConfig = namedArgs[StartupAssemblyArgument].Replace(".dll", ".runtimeconfig.json");
        var commandName = args[0];

        var subProcessArguments = new string[args.Length - 1];
        if (subProcessArguments.Length > 0)
        {
            Array.Copy(args, 1, subProcessArguments, 0, subProcessArguments.Length);
        }

        var subProcessCommandLine =
            $"exec --depsfile {EscapePath(depsFile)} " +
            $"--runtimeconfig {EscapePath(runtimeConfig)} " +
            $"--additional-deps AsyncAPI.Saunter.Generator.Cli.deps.json " +
            //$"--additionalprobingpath {EscapePath(typeof(Program).GetTypeInfo().Assembly.Location)} " +
            $"{EscapePath(typeof(Program).GetTypeInfo().Assembly.Location)} " +
            $"_{commandName} {string.Join(" ", subProcessArguments.Select(EscapePath))}";

        try
        {
            var subProcess = Process.Start("dotnet", subProcessCommandLine);
            subProcess.WaitForExit();
            return subProcess.ExitCode;
        }
        catch (Exception e)
        {
            throw new Exception("Running internal _tofile failed.", e);
        }
    };

    private static string EscapePath(string path)
    {
        return path.Contains(' ') ? "\"" + path + "\"" : path;
    }
}
