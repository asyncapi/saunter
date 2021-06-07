using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NJsonSchema.Generation;
using Saunter.Utils;

namespace Saunter.Generation
{
    public static class SaunterServiceCollectionExtensions
    {
        public static IServiceCollection AddAsyncApiSchemaGeneration(this IServiceCollection services, Action<AsyncApiOptions> setupAction)
        {
            services.AddOptions();

            services.TryAddTransient<IAsyncApiDocumentSerializer, JsonAsyncApiDocumentSerializer>();
            services.TryAddTransient<IAsyncApiDocumentProvider, AsyncApiDocumentProvider>();
            services.TryAddTransient<IDocumentGenerator, DocumentGenerator>();
            services.TryAddTransient<JsonSchemaGenerator>();
            services.TryAddSingleton<JsonSchemaGeneratorSettings>();

            if (setupAction != null) services.Configure(setupAction);

            return services;
        }
    }
}