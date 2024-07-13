using AsyncAPI.Saunter.Generator.Cli.ToFile;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.Community.Logging;
using Shouldly;

namespace AsyncAPI.Saunter.Generator.Cli.Tests.ToFile;

public class StreamProviderTests
{
    private readonly IStreamProvider _streamProvider;
    private readonly ILogger<StreamProvider> _logger;

    public StreamProviderTests()
    {
        this._logger = Substitute.For<ILogger<StreamProvider>>();
        this._streamProvider = new StreamProvider(this._logger);
    }

    [Fact]
    public void NullPathIsStdOut()
    {
        using var stream = this._streamProvider.GetStreamFor(null);

        stream.ShouldNotBeNull();
        Assert.False(stream is FileStream);
        this._logger.Received(1).CallToLog(LogLevel.Debug);
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

        this._logger.Received(1).CallToLog(LogLevel.Debug);
    }
}
