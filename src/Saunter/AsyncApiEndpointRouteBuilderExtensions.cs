#if !NETSTANDARD2_0
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
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
            var options= endpoints.ServiceProvider.GetService<IOptions<AsyncApiOptions>>();
            var route = options?.Value?.Middleware?.Route ?? AsyncApiMiddlewareOptions.AsyncApiMiddlewareDefaultRoute;

            // Add the endpoint
            return endpoints.Map(route, pipeline).WithDisplayName("Async API Documentation");
        }
    }
}
#endif