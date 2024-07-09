using System;
using System.Linq;
using LEGO.AsyncAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Saunter.AttributeProvider.Attributes;

namespace Saunter.IntegrationTests.ReverseProxy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => logging.AddSimpleConsole(console => console.SingleLine = true))
                .ConfigureWebHostDefaults(web =>
                {
                    web.UseStartup<Startup>();
                    web.UseUrls("http://*:5000");
                });
        }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAsyncApiSchemaGeneration(options =>
            {
                options.AssemblyMarkerTypes = new[] { typeof(StreetlightsController) };

                options.AsyncApi = new AsyncApiDocument
                {
                    Info = new AsyncApiInfo
                    {
                        Title = Environment.GetEnvironmentVariable("PATH_BASE"),
                        Version = "1.0.0"
                    }
                };
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            // If running behind a reverse-proxy, you will be responsible for setting the context.Request.PathBase somehow.
            // In this example we use an environment variable which is set from the docker-compose file.
            app.Use((context, next) =>
            {
                context.Request.PathBase = new PathString(Environment.GetEnvironmentVariable("PATH_BASE"));
                return next();
            });

            app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAsyncApiDocuments();
                endpoints.MapAsyncApiUi();

                endpoints.MapControllers();
            });

            // Print the AsyncAPI doc location
            var logger = app.ApplicationServices.GetService<ILoggerFactory>().CreateLogger<Program>();
            var addresses = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses;

            logger.LogInformation("AsyncAPI doc available at: {URL}", $"{addresses.FirstOrDefault()}/asyncapi/asyncapi.json");
            logger.LogInformation("AsyncAPI UI available at: {URL}", $"{addresses.FirstOrDefault()}/asyncapi/ui/");
        }
    }


    public class LightMeasuredEvent
    {
        public int Id { get; set; }
        public int Lumens { get; set; }
    }

    [AsyncApi]
    [ApiController]
    [Route("")]
    public class StreetlightsController
    {
        [Channel("publish/light/measured"), PublishOperation(typeof(LightMeasuredEvent))]
        [HttpPost, Route("publish/light/measured")]
        public void MeasureLight([FromBody] LightMeasuredEvent _)
        {
        }
    }
}
