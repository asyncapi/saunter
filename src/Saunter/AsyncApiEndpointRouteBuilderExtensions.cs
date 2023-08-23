using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Saunter.UI;

namespace Saunter
{
    public static class AsyncApiEndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Maps the AsyncAPI document endpoint
        /// </summary>
        ///
        [Obsolete("This property is obsolete. Use UseAsyncApi instead.", false)]
        public static IEndpointConventionBuilder MapAsyncApiDocuments(
            this IEndpointRouteBuilder endpoints)
        {
            var pipeline = endpoints.CreateApplicationBuilder()
                .UseMiddleware<AsyncApiMiddleware>()
                .Build();

            var options = endpoints.ServiceProvider.GetService<IOptions<AsyncApiOptions>>();
            var route = options.Value.Middleware.Route;

            return endpoints.MapGet(route, pipeline);
        }


        /// <summary>
        /// Maps the AsyncAPI UI endpoint(s)
        /// </summary>
        [Obsolete("This property is obsolete. Use UseAsyncApi instead.", false)]
        public static IEndpointConventionBuilder MapAsyncApiUi(this IEndpointRouteBuilder endpoints)
        {
            var pipeline = endpoints.CreateApplicationBuilder()
                // I don't really understand why...
                // https://github.com/dotnet/aspnetcore/issues/24252#issuecomment-663620294
                .Use((context, next) =>
                {
                    context.SetEndpoint(null);
                    return next();
                })
                .UseMiddleware<AsyncApiUiMiddleware>()
                .Build();

            var options = endpoints.ServiceProvider.GetService<IOptions<AsyncApiOptions>>();
            var route = options.Value.Middleware.UiBaseRoute + "{*wildcard}";

            return endpoints.MapGet(route, pipeline);
        }
    }
}