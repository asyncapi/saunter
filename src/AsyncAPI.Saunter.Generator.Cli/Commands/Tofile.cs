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

        var assembly = typeof(Program).GetTypeInfo().Assembly;
        var subProcessCommandLine =
            $"exec --depsfile {EscapePath(depsFile)} " +
            $"--runtimeconfig {EscapePath(runtimeConfig)} " +
            $"{EscapePath(assembly.Location)} " +
            $"_{commandName} {string.Join(" ", subProcessArguments.Select(EscapePath))}";

        var subProcess = Process.Start("dotnet", subProcessCommandLine);
        subProcess.WaitForExit();
        return subProcess.ExitCode;
    };

    private static string EscapePath(string path)
    {
        return (path.Contains(' ') || string.IsNullOrWhiteSpace(path)) ? "\"" + path + "\"" : path;
    }
}
