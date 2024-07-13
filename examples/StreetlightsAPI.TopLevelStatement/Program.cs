using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Web;
using Saunter;
using Saunter.AsyncApiSchema.v2;
using StreetlightsAPI;

LogManager.Setup().LoadConfigurationFromAppSettings();

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSimpleConsole(console => console.SingleLine = true);
builder.Host.UseNLog();

// Add Saunter to the application services. 
builder.Services.AddAsyncApiSchemaGeneration(options =>
{
    options.AssemblyMarkerTypes = [typeof(StreetlightMessageBus)];

    options.Middleware.UiTitle = "Streetlights API";

    options.AsyncApi = new AsyncApiDocument
    {
        Info = new Info("Streetlights API", "1.0.0")
        {
            Description = "The Smartylighting Streetlights API allows you to remotely manage the city lights.",
            License = new License("Apache 2.0")
            {
                Url = "https://www.apache.org/licenses/LICENSE-2.0"
            }
        },
        Servers =
        {
            ["mosquitto"] = new Server("test.mosquitto.org", "mqtt"),
            ["webapi"] = new Server("localhost:5000", "http"),
        },
    };
});

builder.Services.AddScoped<IStreetlightMessageBus, StreetlightMessageBus>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseRouting();
app.UseCors(configure => configure.AllowAnyOrigin().AllowAnyMethod());

// to be fixed with issue #173
#pragma warning disable ASP0014 // Suggest using top level route registrations instead of UseEndpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapAsyncApiDocuments();
    endpoints.MapAsyncApiUi();

    endpoints.MapControllers();
});
#pragma warning restore ASP0014 // Suggest using top level route registrations instead of UseEndpoints

await app.StartAsync();

// Print the AsyncAPI doc location
var logger = app.Services.GetService<ILoggerFactory>().CreateLogger<Program>();
var options = app.Services.GetService<IOptions<AsyncApiOptions>>();
var addresses = app.Urls;
logger.LogInformation("AsyncAPI doc available at: {URL}", $"{addresses.FirstOrDefault()}{options.Value.Middleware.Route}");
logger.LogInformation("AsyncAPI UI available at: {URL}", $"{addresses.FirstOrDefault()}{options.Value.Middleware.UiBaseRoute}");

// Redirect base url to AsyncAPI UI
app.Map("/", () => Results.Redirect("index.html"));
app.Map("/index.html", () => Results.Redirect(options.Value.Middleware.UiBaseRoute));

await app.WaitForShutdownAsync();
