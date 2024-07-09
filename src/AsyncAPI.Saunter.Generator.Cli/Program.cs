using AsyncAPI.Saunter.Generator.Cli.ToFile;
using ConsoleAppFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();
services.AddLogging(builder => builder.AddSimpleConsole(x => x.SingleLine = true).SetMinimumLevel(LogLevel.Trace));
services.AddToFileCommand();

using var serviceProvider = services.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
ConsoleApp.LogError = msg => logger.LogError(msg);
ConsoleApp.ServiceProvider = serviceProvider;

var app = ConsoleApp.Create();
app.Add<ToFileCommand>();
app.Run(args);
