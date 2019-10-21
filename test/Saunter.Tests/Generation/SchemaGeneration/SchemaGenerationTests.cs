using Microsoft.Extensions.Options;
using Saunter.Generation;
using Saunter.Generation.SchemaGeneration;
using Shouldly;
using Xunit;

namespace Saunter.Tests.Generation.SchemaGeneration
{
    public class SchemaGenerationTests
    {
        private readonly ISchemaRepository _schemaRepository;
        private readonly SchemaGenerator _schemaGenerator;
        
        public SchemaGenerationTests()
        {
            _schemaRepository = new SchemaRepository();
            var options = new AsyncApiDocumentGeneratorOptions();
            
            _schemaGenerator = new SchemaGenerator(Options.Create(options));
        }
        
        [Fact]
        public void Test1()
        {
            var type = typeof(Foo);

            var schema = _schemaGenerator.GenerateSchema(type, _schemaRepository);
            
            schema.ShouldNotBeNull();
        }
        
    }

    public class Foo
    {
        public int Id { get; set; }

        public Bar Bar { get; set; }   
    }

    public class Bar
    {
        public string Name { get; set; }

        public decimal? Cost { get; set; }
    }
    
}