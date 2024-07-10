namespace AsyncAPI.Saunter.Generator.Cli.ToFile;

internal interface IStreamProvider
{
    Stream GetStreamFor(string path);
}

internal class StreamProvider : IStreamProvider
{
    public Stream GetStreamFor(string path)
    {
        if (!string.IsNullOrEmpty(path))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        return path != null ? File.Create(path) : Console.OpenStandardOutput();
    }
}
