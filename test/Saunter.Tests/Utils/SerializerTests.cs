using System.Reflection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema.Generation;
using Saunter.AsyncApiSchema.v2;
using Saunter.Generation;
using Saunter.Generation.SchemaGeneration;
using Saunter.Tests.Generation.DocumentGeneratorTests;
using Saunter.Utils;
using Xunit;

namespace Saunter.Tests.Utils
{
    public abstract class SerializerTests
    {
        private readonly DocumentGenerator _documentGenerator;

        public SerializerTests()
        {
            var options = new AsyncApiOptions();
            var schemaGenerator = new JsonSchemaGenerator(options.JsonSchemaGeneratorSettings);
            _documentGenerator = new DocumentGenerator(Options.Create(options), schemaGenerator, options.JsonSchemaGeneratorSettings);
        }

        protected abstract IAsyncApiDocumentSerializer CreateSerializer();

        [Fact]
        public void TestSerialize()
        {
            var doc = _documentGenerator.GenerateDocument(new[]
                {typeof(MethodAttributesTests.TenantMessagePublisher).GetTypeInfo()});
            var serializedDoc = CreateSerializer().Serialize(doc);
        }
    }

    //public class SystemTextJsonSerializerTests : SerializerTests
    //{
    //    protected override IAsyncApiDocumentSerializer CreateSerializer()
    //    {
    //        return new SystemTextJsonAsyncApiDocumentSerializer();
    //    }
    //}


    public class NewtonsoftSerializerTests : SerializerTests
    {
        protected override IAsyncApiDocumentSerializer CreateSerializer()
        {
            return new NewtonsoftAsyncApiDocumentSerializer();
        }
    }
}
