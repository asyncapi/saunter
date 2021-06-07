using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NJsonSchema.Converters;

namespace Saunter.AsyncApiSchema.v2
{
    [JsonConverter(typeof(JsonReferenceConverter))]
    public class AsyncApiDocument
    {
        /// <summary>
        /// Specifies the AsyncAPI Specification version being used.
        /// </summary>
        [JsonProperty("asyncapi")]
        public AsyncApiVersionString AsyncApi { get; } = AsyncApiVersionString.v2;
        
        /// <summary>
        /// Identifier of the application the AsyncAPI document is defining.
        /// </summary>
        [JsonProperty("id")]
        public Identifier Id { get; set; }

        /// <summary>
        /// Provides metadata about the API. The metadata can be used by the clients if needed.
        /// </summary>
        [JsonProperty("info")]
        public Info Info { get; set; }

        /// <summary>
        /// Provides connection details of servers.
        /// </summary>
        [JsonProperty("servers")]
        public Servers Servers { get; } = new Servers();

        /// <summary>
        /// A string representing the default content type to use when encoding/decoding a message's payload.
        /// The value MUST be a specific media type (e.g. application/json).
        /// </summary>
        [JsonProperty("defaultContentType")]
        public string DefaultContentType { get; set; } = "application/json";

        /// <summary>
        /// The available channels and messages for the API.
        /// </summary>
        [JsonProperty("channels")]
        public Channels Channels { get; set; } = new Channels();

        /// <summary>
        /// An element to hold various schemas for the specification.
        /// </summary>
        [JsonProperty("components")]
        public Components Components { get; } = new Components();

        /// <summary>
        /// A list of tags used by the specification with additional metadata.
        /// Each tag name in the list MUST be unique.
        /// </summary>
        [JsonProperty("tags")]
        public ISet<Tag> Tags { get; } = new HashSet<Tag>();

        /// <summary>
        /// Additional external documentation.
        /// </summary>
        [JsonProperty("externalDocs")]
        public ExternalDocumentation ExternalDocs { get; set; }
    }
}