using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Saunter.Microsoft.Extensions.DependencyInjection
{
    public static class SaunterServiceCollectionExtensions
    {
        public static IServiceCollection AddSaunter(this IServiceCollection services, Action<AsyncApiGeneratorOptions> setupAction)
        {
            services.AddOptions();
            
            services.TryAddTransient<IAsyncApiSchemaProvider, AsyncApiSchemaGenerator>();
            
            if (setupAction != null) services.Configure(setupAction);
            
            return services;
        }
    }
}