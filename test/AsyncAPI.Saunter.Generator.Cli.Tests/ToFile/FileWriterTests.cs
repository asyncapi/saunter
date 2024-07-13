using System.Text;
using AsyncAPI.Saunter.Generator.Cli.ToFile;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Xunit.Abstractions;

namespace AsyncAPI.Saunter.Generator.Cli.Tests.ToFile;

public class FileWriterTests
{
    private readonly Action<Stream> _testContextWriter = stream => stream.Write(Encoding.Default.GetBytes("ananas"));

    private readonly FileWriter _writer;
    private readonly IStreamProvider _streamProvider;
    private readonly ILogger<FileWriter> _logger;
    private readonly MemoryStream _stream = new();

    public FileWriterTests(ITestOutputHelper output)
    {
        this._logger = Substitute.For<ILogger<FileWriter>>();
        this._streamProvider = Substitute.For<IStreamProvider>();
        this._streamProvider.GetStreamFor(default).ReturnsForAnyArgs(x =>
        {
            output.WriteLine($"GetStreamFor({x.Args()[0]})");
            return this._stream;
        });
        this._writer = new FileWriter(this._streamProvider, this._logger);
    }

    [Fact]
    public void CheckStreamContents()
    {
        this._writer.Write("/", "", "", "", _testContextWriter);

        this._streamProvider.Received(1).GetStreamFor(Path.GetFullPath("/"));
        Encoding.Default.GetString(this._stream.GetBuffer().Take(6).ToArray()).ShouldBe("ananas");
    }

    [Fact]
    public void CheckName_NoVariablesInTemplate()
    {
        this._writer.Write("/some/path", "fixed_name", "doc", "json", _testContextWriter);

        this._streamProvider.Received(1).GetStreamFor(Path.GetFullPath("/some/path/fixed_name"));
    }

    [Theory]
    [InlineData("./")]
    [InlineData("/")]
    [InlineData("/test/")]
    [InlineData("/test/1/2/3/4/")]
    public void CheckOutputPath_BaseOutputPath_Absolute(string path)
    {
        this._writer.Write(path, "document.something", "", "", _testContextWriter);

        this._streamProvider.Received(1).GetStreamFor(Path.GetFullPath($"{path}document.something"));
    }

    [Theory]
    [InlineData(".")]
    [InlineData("")]
    [InlineData("asyncApi/")]
    [InlineData("service-1/")]
    [InlineData("service 1/")]
    [InlineData("service 1/spec")]
    public void CheckOutputPath_BaseOutputPath_Relative(string path)
    {
        this._writer.Write(path, "document.something", "", "", _testContextWriter);

        this._streamProvider.Received(1).GetStreamFor(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), path, "document.something")));
    }

    [Theory]
    [InlineData("json")]
    [InlineData("yml")]
    [InlineData("txt")]
    public void CheckOutputPath_FormatTemplate(string format)
    {
        this._writer.Write("/some/path", "{extension}_name.{extension}", "doc", format, _testContextWriter);

        this._streamProvider.Received(1).GetStreamFor(Path.GetFullPath($"/some/path/{format}_name.{format}"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void CheckOutputPath_FormatTemplate_trimmed(string format)
    {
        this._writer.Write("/some/path", "{extension}_name.{extension}", "doc", format, _testContextWriter);

        this._streamProvider.Received(1).GetStreamFor(Path.GetFullPath("/some/path/name."));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("asyncApi")]
    [InlineData("service-1")]
    [InlineData("service 1")]
    public void CheckOutputPath_DocumentNameTemplate(string documentName)
    {
        this._writer.Write("/some/path", "{document}.something", documentName, "", _testContextWriter);

        this._streamProvider.Received(1).GetStreamFor(Path.GetFullPath($"/some/path/{documentName}.something"));
    }
}
