#if NETCOREAPP3_0
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Saunter
{
    public static class AsyncApiEndpointRouteBuilderExtensions
    {
        private const string AsyncApiDefaultDocumentTitle = "Async API Documentation";

        /// <summary>
        /// Maps the AsyncAPI endpoint.
        /// </summary>
        /// <param name="endpoints">The endpoints.</param>
        /// <param name="title">The document tile.</param>
        /// <returns>The endpoints with AsyncAPI endpoint added.</returns>
        public static IEndpointConventionBuilder MapAsyncApiDocuments(
            this IEndpointRouteBuilder endpoints,
            string title = AsyncApiDefaultDocumentTitle)
        {
            RequestDelegate pipeline = endpoints.CreateApplicationBuilder()
                .UseMiddleware<AsyncApiMiddleware>()
                .Build();

            var options = endpoints.ServiceProvider.GetService<IOptions<AsyncApiOptions>>();
            string route = options?.Value?.Middleware?.Route ?? AsyncApiMiddlewareOptions.AsyncApiMiddlewareDefaultRoute;

            return endpoints.Map(route, pipeline).WithDisplayName(title);
        }
    }
}
#endif
