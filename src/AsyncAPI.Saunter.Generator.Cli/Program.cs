using AsyncApi.Saunter.Generator.Cli.Commands;
using AsyncApi.Saunter.Generator.Cli.SwashbuckleImport;

// Helper to simplify command line parsing etc.
var runner = new CommandRunner("dotnet asyncapi", "AsyncAPI Command Line Tools", Console.Out);

// NOTE: The "dotnet asyncapi tofile" command does not serve the request directly. Instead, it invokes a corresponding
// command (called _tofile) via "dotnet exec" so that the runtime configuration (*.runtimeconfig & *.deps.json) of the
// provided startupassembly can be used instead of the tool's. This is neccessary to successfully load the
// startupassembly and it's transitive dependencies. See https://github.com/dotnet/coreclr/issues/13277 for more.

// > dotnet asyncapi tofile ...
runner.SubCommand("tofile", "retrieves AsyncAPI from a startup assembly, and writes to file ", c =>
{
    c.Argument(StartupAssemblyArgument, "relative path to the application's startup assembly");
    c.Argument(DocArgument, "name of the AsyncAPI doc you want to retrieve, as configured in your startup class");
    c.Option(OutputOption, "relative path where the AsyncAPI will be output, defaults to stdout");
    c.Option(FormatOption, "exports AsyncAPI in json and/or yml format [Default json]");
    c.OnRun(Tofile.Run(args));
});

// > dotnet asyncapi _tofile ... (* should only be invoked via "dotnet exec")
runner.SubCommand("_tofile", "", c =>
{
    c.Argument(StartupAssemblyArgument, "");
    c.Argument(DocArgument, "");
    c.Option(OutputOption, "");
    c.Option(FormatOption, "");
    c.OnRun(TofileInternal.Run);
});

return runner.Run(args);
