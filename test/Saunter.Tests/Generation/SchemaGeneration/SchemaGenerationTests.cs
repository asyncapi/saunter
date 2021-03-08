using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
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
            var options = new AsyncApiOptions();

            _schemaGenerator = new SchemaGenerator(Options.Create(options));
        }

        [Fact]
        public void GenerateSchema_GenerateSchemaFromTypeWithProperties_GeneratesSchemaCorrectly()
        {
            var type = typeof(Foo);

            var schema = _schemaGenerator.GenerateSchema(type, _schemaRepository);

            schema.ShouldNotBeNull();
            _schemaRepository.Schemas.ShouldNotBeNull();
            _schemaRepository.Schemas.ContainsKey("foo").ShouldBeTrue();
            _schemaRepository.Schemas["foo"].Required.Count.ShouldBe(2);
            _schemaRepository.Schemas["foo"].Required.Contains("id").ShouldBeTrue();
            _schemaRepository.Schemas["foo"].Required.Contains("bar").ShouldBeTrue();
            _schemaRepository.Schemas["foo"].Properties.Count.ShouldBe(5);
            _schemaRepository.Schemas["foo"].Properties.ContainsKey("id").ShouldBeTrue();
            _schemaRepository.Schemas["foo"].Properties.ContainsKey("bar").ShouldBeTrue();
            _schemaRepository.Schemas["foo"].Properties.ContainsKey("fooType").ShouldBeTrue();
            _schemaRepository.Schemas["foo"].Properties.ContainsKey("hello").ShouldBeTrue();
            _schemaRepository.Schemas["foo"].Properties.ContainsKey("world").ShouldBeTrue();
            _schemaRepository.Schemas.ContainsKey("bar").ShouldBeTrue();
            _schemaRepository.Schemas["bar"].Properties.Count.ShouldBe(2);
            _schemaRepository.Schemas["bar"].Properties.ContainsKey("name").ShouldBeTrue();
            _schemaRepository.Schemas["bar"].Properties.ContainsKey("cost").ShouldBeTrue();
        }

        [Fact]
        public void GenerateSchema_GenerateSchemaFromTypeWithFields_GeneratesSchemaCorrectly()
        {
            var type = typeof(Book);

            var schema = _schemaGenerator.GenerateSchema(type, _schemaRepository);

            schema.ShouldNotBeNull();
            _schemaRepository.Schemas.ShouldNotBeNull();
            _schemaRepository.Schemas.ContainsKey("book").ShouldBeTrue();
            _schemaRepository.Schemas["book"].Properties.Count.ShouldBe(4);
            _schemaRepository.Schemas.ContainsKey("foo").ShouldBeTrue();
            _schemaRepository.Schemas["foo"].Required.Count.ShouldBe(2);
            _schemaRepository.Schemas["foo"].Required.Contains("id").ShouldBeTrue();
            _schemaRepository.Schemas["foo"].Required.Contains("bar").ShouldBeTrue();
            _schemaRepository.Schemas["foo"].Properties.Count.ShouldBe(5);
            _schemaRepository.Schemas["foo"].Properties.ContainsKey("id").ShouldBeTrue();
            _schemaRepository.Schemas["foo"].Properties.ContainsKey("bar").ShouldBeTrue();
            _schemaRepository.Schemas["foo"].Properties.ContainsKey("fooType").ShouldBeTrue();
            _schemaRepository.Schemas["foo"].Properties.ContainsKey("hello").ShouldBeTrue();
            _schemaRepository.Schemas["foo"].Properties.ContainsKey("world").ShouldBeTrue();
        }
    }

    public class Foo
    {
        [Required]
        public Guid Id { get; set; }

        [JsonIgnore]
        public string Ignore { get; set; }

        [Required]
        public Bar Bar { get; set; }

        [JsonPropertyName("hello")]
        public string HelloWorld { get; set; }

        [DataMember(Name = "myworld")]
        public string World { get; set; }

        public FooType FooType { get; set; }
    }

    public enum FooType
    {
        Foo,
        Bar
    }

    public class Bar
    {
        public string Name { get; set; }

        public decimal? Cost { get; set; }
    }

    public class Book
    {
        public readonly string Name;

        public readonly string Author;

        public readonly int NumberOfPages;

        public readonly Foo Foo;

        public Book(string name, string author, int numberOfPages, Foo foo)
        {
            Author = author;
            Name = name;
            NumberOfPages = numberOfPages;
            Foo = foo;
        }
    }
}