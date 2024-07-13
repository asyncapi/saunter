using System.Runtime.Loader;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.Extensions.Logging;

namespace AsyncAPI.Saunter.Generator.Cli.ToFile;

internal interface IServiceProviderBuilder
{
    IServiceProvider BuildServiceProvider(string startupAssembly);
}

internal class ServiceProviderBuilder(ILogger<ServiceProviderBuilder> logger) : IServiceProviderBuilder
{
    public IServiceProvider BuildServiceProvider(string startupAssembly)
    {
        var fullPath = Path.GetFullPath(startupAssembly);
        var basePath = Path.GetDirectoryName(fullPath);
        DependencyResolver.Init(basePath);

        logger.LogInformation($"Loading startup assembly: {fullPath}");
        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(fullPath);
        var reporter = new OperationReporter(new OperationReportHandler(
            m => logger.LogError(m),
            m => logger.LogWarning(m),
            m => logger.LogInformation(m),
            m => logger.LogDebug(m)));
        var appServiceProvider = new AppServiceProviderFactory(assembly, reporter);
        var serviceProvider = appServiceProvider.Create([]);

        return serviceProvider;
    }
}
