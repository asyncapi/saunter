using AsyncAPI.Saunter.Generator.Cli.ToFile;
using Shouldly;

namespace AsyncAPI.Saunter.Generator.Cli.Tests.ToFile;

public class StreamProviderTests
{
    private readonly IStreamProvider _streamProvider = new StreamProvider();

    [Fact]
    public void NullPathIsStdOut()
    {
        using var stream = this._streamProvider.GetStreamFor(null);

        stream.ShouldNotBeNull();
        Assert.False(stream is FileStream);
    }

    [Fact]
    public void StringPathIsFileStream()
    {
        var path = Path.GetFullPath("./test");
        File.Delete(path);
        try
        {
            using var stream = this._streamProvider.GetStreamFor(path);

            stream.ShouldNotBeNull();
            Assert.True(stream is FileStream);
            File.Exists(path);
        }
        finally
        {
            File.Delete(path);
        }
    }
}
