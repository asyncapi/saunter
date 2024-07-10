namespace AsyncAPI.Saunter.Generator.Cli.ToFile;

internal interface IStreamProvider
{
    Stream GetStreamFor(string path);
}

internal class StreamProvider : IStreamProvider
{
    public Stream GetStreamFor(string path) => path != null ? File.Create(path) : Console.OpenStandardOutput();
}
