using AsyncAPI.Saunter.Generator.Cli.ToFile;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.Community.Logging;
using Saunter;
using Saunter.AsyncApiSchema.v2;
using Saunter.Serialization;
using Shouldly;

namespace AsyncAPI.Saunter.Generator.Cli.Tests.ToFile;

public class AsyncApiDocumentExtractorTests
{
    private readonly AsyncApiDocumentExtractor _extractor;
    private readonly ILogger<AsyncApiDocumentExtractor> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IAsyncApiDocumentProvider _documentProvider;
    private readonly IOptions<AsyncApiOptions> _asyncApiOptions;
    private readonly IAsyncApiDocumentSerializer _documentSerializer;

    public AsyncApiDocumentExtractorTests()
    {
        var services = new ServiceCollection();
        this._documentProvider = Substitute.For<IAsyncApiDocumentProvider>();
        this._asyncApiOptions = Substitute.For<IOptions<AsyncApiOptions>>();
        var options = new AsyncApiOptions();
        this._asyncApiOptions.Value.Returns(options);
        this._documentSerializer = Substitute.For<IAsyncApiDocumentSerializer>();
        services.AddSingleton(this._documentProvider);
        services.AddSingleton(this._asyncApiOptions);
        services.AddSingleton(this._documentSerializer);
        this._serviceProvider = services.BuildServiceProvider();

        this._logger = Substitute.For<ILogger<AsyncApiDocumentExtractor>>();
        this._extractor = new AsyncApiDocumentExtractor(this._logger);
    }

    [Fact]
    public void GetAsyncApiDocument_Null_NoMarkerAssemblies()
    {
        var documents = this._extractor.GetAsyncApiDocument(this._serviceProvider, null).ToList();

        this._logger.Received(1).CallToLog(LogLevel.Critical);
    }

    [Fact]
    public void GetAsyncApiDocument_Default_WithMarkerAssembly()
    {
        this._asyncApiOptions.Value.AssemblyMarkerTypes = [typeof(AsyncApiDocumentExtractorTests)];
        var doc = new AsyncApiDocument();
        this._documentProvider.GetDocument(default, default).ReturnsForAnyArgs(doc);
        this._documentSerializer.Serialize(doc).ReturnsForAnyArgs("""
                                                                  asyncapi: 2.6.0
                                                                  info:
                                                                    title: Streetlights API
                                                                  """);

        var documents = this._extractor.GetAsyncApiDocument(this._serviceProvider, null).ToList();

        this._logger.Received(0).CallToLog(LogLevel.Critical);
        documents.Count.ShouldBe(1);
        documents[0].name.ShouldBeNull();
        documents[0].document.Info.Title.ShouldBe("Streetlights API");
    }

    [Fact]
    public void GetAsyncApiDocument_1NamedDocument()
    {
        this._asyncApiOptions.Value.AssemblyMarkerTypes = [typeof(AsyncApiDocumentExtractorTests)];
        var doc = new AsyncApiDocument();
        this._asyncApiOptions.Value.NamedApis["service 1"] = doc;
        this._documentProvider.GetDocument(default, default).ReturnsForAnyArgs(doc);
        this._documentSerializer.Serialize(doc).ReturnsForAnyArgs("""
                                                                  asyncapi: 2.6.0
                                                                  info:
                                                                    title: Streetlights API
                                                                  """);

        var documents = this._extractor.GetAsyncApiDocument(this._serviceProvider, null).ToList();

        this._logger.Received(0).CallToLog(LogLevel.Critical);
        documents.Count.ShouldBe(1);
        documents[0].name.ShouldBe("service 1");
        documents[0].document.Info.Title.ShouldBe("Streetlights API");
    }

    [Fact]
    public void GetAsyncApiDocument_2NamedDocument()
    {
        this._asyncApiOptions.Value.AssemblyMarkerTypes = [typeof(AsyncApiDocumentExtractorTests)];
        var doc1 = new AsyncApiDocument { Id = "1" };
        var doc2 = new AsyncApiDocument { Id = "2" };
        this._asyncApiOptions.Value.NamedApis["service 1"] = doc1;
        this._asyncApiOptions.Value.NamedApis["service 2"] = doc2;
        this._documentProvider.GetDocument(Arg.Any<AsyncApiOptions>(), Arg.Is(doc1)).Returns(doc1);
        this._documentProvider.GetDocument(Arg.Any<AsyncApiOptions>(), Arg.Is(doc2)).Returns(doc2);
        this._documentSerializer.Serialize(doc1).Returns("""
                                                         asyncapi: 2.6.0
                                                         info:
                                                           title: Streetlights API 1
                                                         """);
        this._documentSerializer.Serialize(doc2).Returns("""
                                                         asyncapi: 2.6.0
                                                         info:
                                                           title: Streetlights API 2
                                                         """);

        var documents = this._extractor.GetAsyncApiDocument(this._serviceProvider, null).OrderBy(x => x.name).ToList();

        this._logger.Received(0).CallToLog(LogLevel.Critical);
        documents.Count.ShouldBe(2);
        documents[0].name.ShouldBe("service 1");
        documents[0].document.Info.Title.ShouldBe("Streetlights API 1");
        documents[1].name.ShouldBe("service 2");
        documents[1].document.Info.Title.ShouldBe("Streetlights API 2");
    }

    [Fact]
    public void GetAsyncApiDocument_LogErrors()
    {
        this._asyncApiOptions.Value.AssemblyMarkerTypes = [typeof(AsyncApiDocumentExtractorTests)];
        var doc = new AsyncApiDocument();
        this._documentProvider.GetDocument(default, default).ReturnsForAnyArgs(doc);
        this._documentSerializer.Serialize(doc).ReturnsForAnyArgs("""
                                                                  asyncapi: 2.6.0
                                                                  info:
                                                                    title: Streetlights API
                                                                  channels:
                                                                    publish/light/measured:
                                                                      servers:
                                                                        - webapi
                                                                      publish:
                                                                        operationId: MeasureLight
                                                                        summary: Inform about environmental lighting conditions for a particular streetlight.
                                                                        tags:
                                                                          - name: Light
                                                                        message:
                                                                          $ref: '#/components/messages/lightMeasuredEvent'
                                                                  """);

        var documents = this._extractor.GetAsyncApiDocument(this._serviceProvider, null).ToList();

        this._logger.Received(0).CallToLog(LogLevel.Critical);
        this._logger.Received(3).CallToLog(LogLevel.Error);
        this._logger.Received(0).CallToLog(LogLevel.Warning);
        documents.Count.ShouldBe(1);
        documents[0].name.ShouldBeNull();
        documents[0].document.Info.Title.ShouldBe("Streetlights API");
    }
}
