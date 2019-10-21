using System;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Saunter.Generation;
using Saunter.Generation.SchemaGeneration;

namespace Saunter.Tests.Generation.SchemaGeneration
{
    [TestFixture]
    public class SchemaGenerationTests
    {
        private ISchemaRepository _schemaRepository;
        private AsyncApiDocumentGeneratorOptions _options;
        private SchemaGenerator _schemaGenerator;
        
        [SetUp]
        public void SetUp()
        {
            _schemaRepository = new SchemaRepository();
            _options = new AsyncApiDocumentGeneratorOptions();
            
            _schemaGenerator = new SchemaGenerator(Options.Create(_options));
        }
        
        [Test]
        public void Test1()
        {
            var type = typeof(Foo);

            var schema = _schemaGenerator.GenerateSchema(type, _schemaRepository);
            
            Assert.That(schema, Is.Not.Null);
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