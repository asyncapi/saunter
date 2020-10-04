using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Saunter
{
    public static class AsyncApiEndpointRouteBuilderExtensions
    {
        public static IEndpointConventionBuilder MapAsyncApiDocuments(
            this IEndpointRouteBuilder endpoints)
        {
            // Add the middleware 
            var pipeline = endpoints.CreateApplicationBuilder()
                .UseMiddleware<AsyncApiMiddleware>()
                .Build();

            // Retrieve the route from options. If options can't be retrieved, use default constant
            var route = AsyncApiMiddlewareOptions.AsyncApiMiddlewareDefaultRoute;
            var options= endpoints.ServiceProvider.GetService(typeof(IOptions<AsyncApiOptions>)) as IOptions<AsyncApiOptions>;
            route = options?.Value.Middleware.Route;

            // Add the endpoint
            return endpoints.Map(route, pipeline).WithDisplayName("Async API Documentation");
        }
    }
}
