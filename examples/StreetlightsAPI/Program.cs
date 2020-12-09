using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Saunter;
using Saunter.AsyncApiSchema.v2;
using Saunter.Generation;

namespace StreetlightsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<Startup>();
                    builder.UseUrls("http://localhost:5000");
                });
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Saunter to the application services. 
            services.AddAsyncApiSchemaGeneration(options =>
            {
                options.AssemblyMarkerTypes = new[] { typeof(StreetlightMessageBus) };

                options.AsyncApi = new AsyncApiDocument(new Info("Streetlights API", "1.0.0")
                {
                    Description = "The Smartylighting Streetlights API allows you\nto remotely manage the city lights.",
                    License = new License("Apache 2.0")
                    {
                        Url = "https://www.apache.org/licenses/LICENSE-2.0"
                    }
                })
                {
                    Servers =
                    {
                        { "mosquitto", new Server("test.mosquitto.org", "mqtt") }
                    }
                };
            });


            services.AddScoped<IStreetlightMessageBus, StreetlightMessageBus>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapAsyncApiDocuments();
            });
        }
    }
}
