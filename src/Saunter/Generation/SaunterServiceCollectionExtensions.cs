using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Saunter.Generation.SchemaGeneration;

namespace Saunter.Generation
{
    public static class SaunterServiceCollectionExtensions
    {
        public static IServiceCollection AddAsyncApiSchemaGeneration(this IServiceCollection services, Action<AsyncApiDocumentGeneratorOptions> setupAction)
        {
            services.AddOptions();
            
            services.TryAddTransient<IAsyncApiDocumentProvider, AsyncApiDocumentGenerator>();
            services.TryAddTransient<ISchemaGenerator, NoOpSchemaGenerator>();
            
            if (setupAction != null) services.Configure(setupAction);
            
            return services;
        }
    }
}