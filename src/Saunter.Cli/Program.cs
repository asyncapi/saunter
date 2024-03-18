using Saunter;
using Saunter.Cli;

ConsoleAppBuilder builder = ConsoleApp.CreateBuilder(args);

builder.ConfigureServices(s => s.AddAsyncApiSchemaGeneration());

ConsoleApp app = builder.Build();

app.AddSubCommands<SpecificationGeneratorCommand>();

app.Run();
