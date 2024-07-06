using AsyncApi.Saunter.Generator.Cli.Commands;
using AsyncApi.Saunter.Generator.Cli.SwashbuckleImport;
using AsyncAPI.Saunter.Generator.Cli.Internal;

DependencyResolver.Init();

// Helper to simplify command line parsing etc.
var runner = new CommandRunner("dotnet asyncapi.net", "AsyncAPI Command Line Tools", Console.Out);

// NOTE: The "dotnet asyncapi tofile" command does not serve the request directly. Instead, it invokes a corresponding
// command (called _tofile) via "dotnet exec" so that the runtime configuration (*.runtimeconfig & *.deps.json) of the
// provided startupassembly can be used instead of the tool's. This is neccessary to successfully load the
// startupassembly and it's transitive dependencies. See https://github.com/dotnet/coreclr/issues/13277 for more.

// > dotnet asyncapi.net tofile ...
runner.SubCommand("tofile", "retrieves AsyncAPI from a startup assembly, and writes to file ", c =>
{
    c.Argument(StartupAssemblyArgument, "relative path to the application's startup assembly");
    c.Option(DocOption, "name(s) of the AsyncAPI documents you want to retrieve, as configured in your startup class [defaults to all documents]");
    c.Option(OutputOption, "relative path where the AsyncAPI will be output [defaults to stdout]");
    c.Option(FileNameOption, "defines the file name template, {document} and {extension} template variables can be used [defaults to \"{document}_asyncapi.{extension}\"]");
    c.Option(FormatOption, "exports AsyncAPI in json and/or yml format [defaults to json]");
    c.Option(EnvOption, "define environment variable(s) for the application during generation of the AsyncAPI files [defaults to empty, can be used to define for example ASPNETCORE_ENVIRONMENT]");
    c.OnRun(Tofile.Run(args));
});

// > dotnet asyncapi.net _tofile ... (* should only be invoked via "dotnet exec")
runner.SubCommand("_tofile", "", c =>
{
    c.Argument(StartupAssemblyArgument, "");
    c.Option(DocOption, "");
    c.Option(OutputOption, "");
    c.Option(FileNameOption, "");
    c.Option(FormatOption, "");
    c.Option(EnvOption, "");
    c.OnRun(TofileInternal.Run);
});

return runner.Run(args);
