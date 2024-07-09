using System;
using LEGO.AsyncAPI.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Saunter.AttributeProvider;
using Saunter.Options;
using Saunter.SharedKernel;
using Saunter.SharedKernel.Interfaces;

namespace Saunter
{
    public static class AsyncApiServiceCollectionExtensions
    {
        /// <summary>
        /// Add required services for AsyncAPI schema generation to the service collection.
        /// </summary>
        /// <param name="services">The collection to add services to.</param>
        /// <param name="setupAction">An action used to configure the AsyncAPI options.</param>
        /// <returns>The service collection so additional calls can b e chained.</returns>
        public static IServiceCollection AddAsyncApiSchemaGeneration(this IServiceCollection services, Action<AsyncApiOptions>? setupAction = null)
        {
            services.AddOptions();

            services.TryAddSingleton<IAsyncApiDocumentCloner, AsyncApiDocumentSerializeCloner>();
            services.TryAddSingleton<IAsyncApiSchemaGenerator, AsyncApiSchemaGenerator>();

            services.TryAddTransient<IAsyncApiDocumentProvider, AttributeDocumentProvider>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return services;
        }

        /// <summary>
        /// Add a named AsyncAPI document to the service collection.
        /// </summary>
        /// <param name="services">The collection to add the document to.</param>
        /// <param name="documentName">The name used to refer to the document. Used in the <see cref="AttributeProvider.Attributes.AsyncApiAttribute"/> and in middleware HTTP paths.</param>
        /// <param name="setupAction">An action used to configure the named document.</param>
        /// <returns>The service collection so additional calls can be chained.</returns>
        public static IServiceCollection ConfigureNamedAsyncApi(this IServiceCollection services, string documentName, Action<AsyncApiDocument> setupAction)
        {
            services.Configure<AsyncApiOptions>(options =>
            {
                if (options.Middleware.Route == null
                    || !options.Middleware.Route.ToLower().Contains("{document}")
                    || options.Middleware.UiBaseRoute == null
                    || !options.Middleware.UiBaseRoute.ToLower().Contains("{document}"))
                {
                    options.Middleware.Route = "/asyncapi/{document}/asyncapi.json";
                    options.Middleware.UiBaseRoute = "/asyncapi/{document}/ui/";
                }

                var document = options.NamedApis.GetOrAdd(documentName, _ => new AsyncApiDocument());

                setupAction(document);
            });
            return services;
        }
    }
}
