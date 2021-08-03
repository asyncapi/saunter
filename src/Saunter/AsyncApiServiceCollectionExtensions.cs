using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Saunter.AsyncApiSchema.v2;
using Saunter.Generation;
using Saunter.Serialization;
using Saunter.Utils;

namespace Saunter
{
    public static class AsyncApiServiceCollectionExtensions
    {
        public static IServiceCollection AddAsyncApiSchemaGeneration(this IServiceCollection services, Action<AsyncApiOptions> setupAction = null)
        {
            services.AddOptions();

            services.TryAddTransient<IAsyncApiDocumentProvider, AsyncApiDocumentProvider>();
            services.TryAddTransient<IDocumentGenerator, DocumentGenerator>();
            services.TryAddTransient<IAsyncApiDocumentSerializer, NewtonsoftAsyncApiDocumentSerializer>();

            if (setupAction != null) services.Configure(setupAction);

            return services;
        }

        public static IServiceCollection ConfigureNamedAsyncApi(this IServiceCollection services, string documentName, Action<AsyncApiDocument> setupAction)
        {
            services.Configure<AsyncApiOptions>(options =>
            {
                options.Middleware.Route = "/asyncapi/{document}/asyncapi.json";
                options.Middleware.UiBaseRoute = "/asyncapi/{document}/ui/";
                var document = options.NamedApis.GetOrAdd(documentName, _ => new AsyncApiDocument() {DocumentName = documentName});

                setupAction(document);
            });
            return services;
        }
    }
}