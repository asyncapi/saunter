using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Saunter.UI;

using System.Linq;

namespace Saunter;

public static class AsyncApiEndpointRouteBuilderExtensions
{
    /// <summary>
    /// Maps the AsyncAPI document endpoint
    /// </summary>
    public static IEndpointConventionBuilder MapAsyncApiDocuments(
        this IEndpointRouteBuilder endpoints)
    {
        RequestDelegate pipeline = endpoints.CreateApplicationBuilder()
            .UseMiddleware<AsyncApiMiddleware>()
            .Build();

        IOptions<AsyncApiOptions> options = endpoints.ServiceProvider.GetRequiredService<IOptions<AsyncApiOptions>>();

        string route = options.Value.Middleware.Route;

        if (options.Value.NamedApis.Any())
        {
            endpoints.MapGet(route.Replace("/{document}", null), pipeline);
        }

        return endpoints.MapGet(route, pipeline);
    }


    /// <summary>
    /// Maps the AsyncAPI UI endpoint(s)
    /// </summary>
    public static IEndpointConventionBuilder MapAsyncApiUi(this IEndpointRouteBuilder endpoints)
    {
        RequestDelegate pipeline = endpoints.CreateApplicationBuilder()
            // I don't really understand why...
            // https://github.com/dotnet/aspnetcore/issues/24252#issuecomment-663620294
            .Use((context, next) =>
            {
                context.SetEndpoint(null);
                return next();
            })
            .UseMiddleware<AsyncApiUiMiddleware>()
            .Build();

        IOptions<AsyncApiOptions> options = endpoints.ServiceProvider.GetRequiredService<IOptions<AsyncApiOptions>>();
        string route = options.Value.Middleware.UiBaseRoute + "{*wildcard}";

        if (options.Value.NamedApis.Any())
        {
            endpoints.MapGet(route.Replace("/{document}", null), pipeline);
        }

        return endpoints.MapGet(route, pipeline);
    }
}