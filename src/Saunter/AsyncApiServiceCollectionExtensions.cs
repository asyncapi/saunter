using Microsoft.Extensions.DependencyInjection;
using Saunter.Utils;

namespace Saunter
{
    public static class AsyncApiServiceCollectionExtensions
    {
        public static IServiceCollection AddAsyncApi(this IServiceCollection services)
        {
            services.AddTransient<IAsyncApiDocumentSerializer, JsonAsyncApiDocumentSerializer>();

            return services;
        }
    }
}