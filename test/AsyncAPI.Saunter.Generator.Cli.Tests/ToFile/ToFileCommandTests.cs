using AsyncAPI.Saunter.Generator.Cli.ToFile;
using LEGO.AsyncAPI.Models;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Xunit.Abstractions;

namespace AsyncAPI.Saunter.Generator.Cli.Tests.ToFile;

public class ToFileCommandTests
{
    private readonly ToFileCommand _command;
    private readonly IEnvironmentBuilder _environment;
    private readonly IServiceProviderBuilder _builder;
    private readonly IAsyncApiDocumentExtractor _docExtractor;
    private readonly IFileWriter _fileWriter;
    private readonly ILogger<ToFileCommand> _logger;
    private readonly ITestOutputHelper _output;

    public ToFileCommandTests(ITestOutputHelper output)
    {
        this._output = output;
        this._logger = Substitute.For<ILogger<ToFileCommand>>();
        this._environment = Substitute.For<IEnvironmentBuilder>();
        this._builder = Substitute.For<IServiceProviderBuilder>();
        this._docExtractor = Substitute.For<IAsyncApiDocumentExtractor>();
        this._fileWriter = Substitute.For<IFileWriter>();
        this._command = new ToFileCommand(this._logger, _environment, _builder, _docExtractor, _fileWriter);
    }

    [Fact]
    public void StartupAssembly_FileNotFoundException()
    {
        Assert.Throws<FileNotFoundException>(() => this._command.ToFile(""));
    }

    [Fact]
    public void SetEnvironmentVariables()
    {
        var me = typeof(ToFileCommandTests).Assembly.Location;

        this._command.ToFile(me, env: "env=value");

        this._environment.Received(1).SetEnvironmentVariables("env=value");
    }

    [Fact]
    public void BuildServiceProvider()
    {
        var me = typeof(ToFileCommandTests).Assembly.Location;
        this._output.WriteLine($"Assembly: {me}");

        this._command.ToFile(me);

        this._builder.Received(1).BuildServiceProvider(me);
    }

    [Fact]
    public void GetAsyncApiDocument_DefaultDocParam()
    {
        var me = typeof(ToFileCommandTests).Assembly.Location;
        this._output.WriteLine($"Assembly: {me}");
        var sp = Substitute.For<IServiceProvider>();
        this._builder.BuildServiceProvider(default).ReturnsForAnyArgs(sp);

        this._command.ToFile(me);

        this._docExtractor.Received(1).GetAsyncApiDocument(sp, null);
    }

    [Fact]
    public void GetAsyncApiDocument_DocParam()
    {
        var me = typeof(ToFileCommandTests).Assembly.Location;
        this._output.WriteLine($"Assembly: {me}");
        var sp = Substitute.For<IServiceProvider>();
        this._builder.BuildServiceProvider(default).ReturnsForAnyArgs(sp);

        this._command.ToFile(me, doc: "a");

        this._docExtractor.Received(1).GetAsyncApiDocument(sp, Arg.Is<string[]>(x => x.SequenceEqual(new[] { "a" }))); ;
    }

    [Fact]
    public void GetAsyncApiDocument_DocParamMultiple()
    {
        var me = typeof(ToFileCommandTests).Assembly.Location;
        this._output.WriteLine($"Assembly: {me}");
        var sp = Substitute.For<IServiceProvider>();
        this._builder.BuildServiceProvider(default).ReturnsForAnyArgs(sp);

        this._command.ToFile(me, doc: "a,b, c ,,");

        this._docExtractor.Received(1).GetAsyncApiDocument(sp, Arg.Is<string[]>(x => x.SequenceEqual(new[] { "a", "b", " c " })));
    }

    [Fact]
    public void WriteFile_DefaultParams()
    {
        var me = typeof(ToFileCommandTests).Assembly.Location;
        this._output.WriteLine($"Assembly: {me}");
        this._docExtractor.GetAsyncApiDocument(default, default).ReturnsForAnyArgs([(null, new AsyncApiDocument { Info = new AsyncApiInfo { Title = "a" } })]);

        this._command.ToFile(me);

        this._fileWriter.ReceivedCalls().Count().ShouldBe(1);
        this._fileWriter.Received(1).Write(Path.GetFullPath("./"), "{document}_asyncapi.{extension}", null, "json", Arg.Any<Action<Stream>>());
    }

    [Theory]
    [InlineData("json")]
    [InlineData("yml")]
    [InlineData("yaml")]
    public void WriteFile_FormatParam(string format)
    {
        var me = typeof(ToFileCommandTests).Assembly.Location;
        this._output.WriteLine($"Assembly: {me}");
        this._docExtractor.GetAsyncApiDocument(default, default).ReturnsForAnyArgs([(null, new AsyncApiDocument { Info = new AsyncApiInfo { Title = "a" } })]);

        this._command.ToFile(me, format: format);

        this._fileWriter.ReceivedCalls().Count().ShouldBe(1);
        this._fileWriter.Received(1).Write(Path.GetFullPath("./"), "{document}_asyncapi.{extension}", null, format, Arg.Any<Action<Stream>>());
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void WriteFile_EmptyFormatParamVariants_FallbackToJson(string format)
    {
        var me = typeof(ToFileCommandTests).Assembly.Location;
        this._output.WriteLine($"Assembly: {me}");
        this._docExtractor.GetAsyncApiDocument(default, default).ReturnsForAnyArgs([(null, new AsyncApiDocument { Info = new AsyncApiInfo { Title = "a" } })]);

        this._command.ToFile(me, format: format);

        this._fileWriter.ReceivedCalls().Count().ShouldBe(1);
        this._fileWriter.Received(1).Write(Path.GetFullPath("./"), "{document}_asyncapi.{extension}", null, "json", Arg.Any<Action<Stream>>());
    }

    [Theory]
    [InlineData("a")]
    [InlineData("json1")]
    [InlineData(".json")]
    public void WriteFile_InvalidFormatParam_FallbackToJson(string format)
    {
        var me = typeof(ToFileCommandTests).Assembly.Location;
        this._output.WriteLine($"Assembly: {me}");
        this._docExtractor.GetAsyncApiDocument(default, default).ReturnsForAnyArgs([(null, new AsyncApiDocument { Info = new AsyncApiInfo { Title = "a" } })]);

        this._command.ToFile(me, format: format);

        this._fileWriter.ReceivedCalls().Count().ShouldBe(0);
    }

    [Fact]
    public void WriteFile_FormatParamMultiple()
    {
        var me = typeof(ToFileCommandTests).Assembly.Location;
        this._output.WriteLine($"Assembly: {me}");
        this._docExtractor.GetAsyncApiDocument(default, default).ReturnsForAnyArgs([(null, new AsyncApiDocument { Info = new AsyncApiInfo { Title = "a" } })]);

        this._command.ToFile(me, format: " json , yaml,yml ,,a, ");

        this._fileWriter.ReceivedCalls().Count().ShouldBe(3);
        this._fileWriter.Received(1).Write(Path.GetFullPath("./"), "{document}_asyncapi.{extension}", null, "json", Arg.Any<Action<Stream>>());
        this._fileWriter.Received(1).Write(Path.GetFullPath("./"), "{document}_asyncapi.{extension}", null, "yml", Arg.Any<Action<Stream>>());
        this._fileWriter.Received(1).Write(Path.GetFullPath("./"), "{document}_asyncapi.{extension}", null, "yaml", Arg.Any<Action<Stream>>());
    }

    [Theory]
    [InlineData("doc")]
    [InlineData("{document}")]
    [InlineData("{extension}")]
    [InlineData("{document}.{extension}")]
    public void WriteFile_FileTemplateParam(string template)
    {
        var me = typeof(ToFileCommandTests).Assembly.Location;
        this._output.WriteLine($"Assembly: {me}");
        this._docExtractor.GetAsyncApiDocument(default, default).ReturnsForAnyArgs([(null, new AsyncApiDocument { Info = new AsyncApiInfo { Title = "a" } })]);

        this._command.ToFile(me, filename: template);

        this._fileWriter.ReceivedCalls().Count().ShouldBe(1);
        this._fileWriter.Received(1).Write(Path.GetFullPath("./"), template, null, "json", Arg.Any<Action<Stream>>());
    }

    [Theory]
    [InlineData("./")]
    [InlineData("/")]
    [InlineData("a/")]
    [InlineData("/a/b")]
    public void WriteFile_OutputPathParam(string path)
    {
        var me = typeof(ToFileCommandTests).Assembly.Location;
        this._output.WriteLine($"Assembly: {me}");
        this._docExtractor.GetAsyncApiDocument(default, default).ReturnsForAnyArgs([(null, new AsyncApiDocument { Info = new AsyncApiInfo { Title = "a" } })]);

        this._command.ToFile(me, output: path);

        this._fileWriter.ReceivedCalls().Count().ShouldBe(1);
        this._fileWriter.Received(1).Write(Path.GetFullPath(path), "{document}_asyncapi.{extension}", null, "json", Arg.Any<Action<Stream>>());
    }
}
