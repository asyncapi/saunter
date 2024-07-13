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
            var directory = Path.GetDirectoryName(path);
            var sw = Stopwatch.StartNew();
            do
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception e) when (sw.Elapsed < TimeSpan.FromMilliseconds(250))
                {
                    logger.LogDebug(e, "Retry...");
                    Thread.Sleep(100);
                }
            }
            while (!Directory.Exists(directory));
        }

        return path != null ? File.Create(path) : Console.OpenStandardOutput();
    }
}
