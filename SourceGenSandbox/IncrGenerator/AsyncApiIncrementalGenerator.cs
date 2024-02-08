using Microsoft.CodeAnalysis;

namespace IncrGenerator;

[Generator]
public class AsyncApiIncrementalGenerator : IIncrementalGenerator
{
    private readonly ComponentIncrementalGenerator _incrementalGenerator;

    private const string AsyncApiDocument = $@"
#nullable enable

using AsyncApiLibrary.Schema.v2;

namespace AsyncApi.Net.Generator
{{
    public class {nameof(AsyncApiDocument)} : IAsyncApiDocument
    {{
        public {nameof(AsyncApiDocument)}(Info info){{
            Info = info;
        }}

        /// <summary>
        /// Specifies the AsyncAPI Specification version being used.
        /// </summary>
        public string? AsyncApi {{ get; }} = ""2.6.0"";

        /// <summary>
        /// Identifier of the application the AsyncAPI document is defining.
        /// </summary>
        public string? Id {{ get; set; }}

        /// <summary>
        /// Provides metadata about the API. The metadata can be used by the clients if needed.
        /// </summary>
        public Info Info {{ get; set; }}

        /// <summary>
        /// Provides connection details of servers.
        /// </summary>
        public Dictionary<string, Server> Servers {{ get; set; }} = [];

        /// <summary>
        /// A string representing the default content type to use when encoding/decoding a message's payload.
        /// The value MUST be a specific media type (e.g. application/json).
        /// </summary>
        public string? DefaultContentType {{ get; set; }} = ""application/json"";

        /// <summary>
        /// The available channels and messages for the API.
        /// </summary>
        public Dictionary<string, ChannelItem> Channels {{ get; set; }} = [];

        /// <summary>
        /// An element to hold various schemas for the specification.
        /// </summary>
        public Components Components {{ get; set; }} = new();

        /// <summary>
        /// A list of tags used by the specification with additional metadata.
        /// Each tag name in the list MUST be unique.
        /// </summary>
        public HashSet<Tag> Tags {{ get; set; }} = [];

        /// <summary>
        /// Additional external documentation.
        /// </summary>
        public ExternalDocumentation? ExternalDocs {{ get; set; }}
    }}
}}
";

    public AsyncApiIncrementalGenerator()
    {
        _incrementalGenerator = new();
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(c =>
            c.AddSource($"{nameof(AsyncApiDocument)}.Generated.cs",
            AsyncApiDocument));

        _incrementalGenerator.Generate("Channel", context);
        _incrementalGenerator.Generate("Operation", context);
        _incrementalGenerator.Generate("JSchema", context);
        _incrementalGenerator.Generate("Message", context);
        _incrementalGenerator.Generate("SecurityScheme", context);
        _incrementalGenerator.Generate("Parameter", context);
        _incrementalGenerator.Generate("CorrelationId", context);
        _incrementalGenerator.Generate("ServerBindings", context);
        _incrementalGenerator.Generate("ChannelBindings", context);
        _incrementalGenerator.Generate("OperationBindings", context);
        _incrementalGenerator.Generate("MessageBindings", context);
        _incrementalGenerator.Generate("OperationTrait", context);
        _incrementalGenerator.Generate("MessageTrait", context);
    }
}
