using Microsoft.Extensions.DependencyInjection;

namespace AsyncAPI.Saunter.Generator.Cli.ToFile;

internal static class ServiceExtensions
{
    public static IServiceCollection AddToFileCommand(this IServiceCollection services)
    {
        services.AddTransient<EnvironmentBuilder>();
        services.AddTransient<ServiceProviderBuilder>();
        services.AddTransient<AsyncApiDocumentExtractor>();
        services.AddTransient<IStreamProvider, StreamProvider>();
        services.AddTransient<FileWriter>();
        return services;
    }
}
