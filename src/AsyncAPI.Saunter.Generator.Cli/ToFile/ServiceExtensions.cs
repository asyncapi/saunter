using Microsoft.Extensions.DependencyInjection;

namespace AsyncAPI.Saunter.Generator.Cli.ToFile;

internal static class ServiceExtensions
{
    public static IServiceCollection AddToFileCommand(this IServiceCollection services)
    {
        services.AddTransient<IEnvironmentBuilder, EnvironmentBuilder>();
        services.AddTransient<IServiceProviderBuilder, ServiceProviderBuilder>();
        services.AddTransient<IAsyncApiDocumentExtractor, AsyncApiDocumentExtractor>();
        services.AddTransient<IStreamProvider, StreamProvider>();
        services.AddTransient<IFileWriter, FileWriter>();
        return services;
    }
}
