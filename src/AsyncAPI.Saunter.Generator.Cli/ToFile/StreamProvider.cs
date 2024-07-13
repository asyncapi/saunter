using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace AsyncAPI.Saunter.Generator.Cli.ToFile;

internal interface IStreamProvider
{
    Stream GetStreamFor(string path);
}

internal class StreamProvider(ILogger<StreamProvider> logger) : IStreamProvider
{
    public Stream GetStreamFor(string path)
    {
        logger.LogDebug($"GetStreamFor(path: {path})");

        if (!string.IsNullOrEmpty(path))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        return path != null ? File.Create(path) : Console.OpenStandardOutput();
    }
}
