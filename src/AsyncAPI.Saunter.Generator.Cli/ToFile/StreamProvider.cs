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
            var directory = new DirectoryInfo(Path.GetDirectoryName(path));
            var sw = Stopwatch.StartNew();
            do
            {
                try
                {
                    directory.Create();
                }
                catch (Exception e) when (sw.Elapsed < TimeSpan.FromMilliseconds(250))
                {
                    logger.LogDebug(e, $"Retry... {directory.Parent.Exists}, {directory.Parent.Parent.Exists}, {directory.Parent.Parent.Parent.Exists}");
                    Thread.Sleep(100);
                }
            }
            while (!directory.Exists);
        }

        return path != null ? File.Create(path) : Console.OpenStandardOutput();
    }
}
