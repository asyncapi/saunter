using System.Reflection;
using Microsoft.Extensions.Options;
using Saunter.Generation;
using Saunter.Serialization;
using Saunter.Tests.Generation.DocumentGeneratorTests;
using Saunter.Utils;
using Shouldly;
using Xunit;

namespace Saunter.Tests.Utils
{
    public abstract class SerializerTests
    {
        private readonly DocumentGenerator _documentGenerator;
        private readonly AsyncApiOptions _options;

        public SerializerTests()
        {
            _options = new AsyncApiOptions();
            _documentGenerator = new DocumentGenerator(Options.Create(_options));
        }

        protected abstract IAsyncApiDocumentSerializer CreateSerializer(IOptions<AsyncApiOptions> options);

        [Fact]
        public void TestSerialize()
        {
            var doc = _documentGenerator.GenerateDocument(new[] { typeof(MethodAttributesTests.TenantMessagePublisher).GetTypeInfo() });
            var serializedDoc = CreateSerializer(Options.Create(_options)).Serialize(doc);

            serializedDoc.ShouldNotBeNullOrWhiteSpace();
        }
    }

    public class NewtonsoftSerializerTests : SerializerTests
    {
        protected override IAsyncApiDocumentSerializer CreateSerializer(IOptions<AsyncApiOptions> options)
        {
            return new NewtonsoftAsyncApiDocumentSerializer(options);
        }
    }
}
