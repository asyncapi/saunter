using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using NJsonSchema;
using NJsonSchema.Generation;
using Shouldly;
using Xunit;

namespace Saunter.Tests.Generation.SchemaGeneration
{
    public class SchemaGenerationTests
    {
        private readonly JsonSchemaResolver _schemaResolver;
        private readonly JsonSchemaGenerator _schemaGenerator;

        public SchemaGenerationTests()
        {
            var settings = new JsonSchemaGeneratorSettings();
            _schemaResolver = new JsonSchemaResolver(new JsonSchema(), settings);
            _schemaGenerator = new JsonSchemaGenerator(settings);
        }

        [Fact]
        public void GenerateSchema_GenerateSchemaFromTypeWithProperties_GeneratesSchemaCorrectly()
        {
            var type = typeof(Foo);

            var schema = _schemaGenerator.Generate(type, _schemaResolver);

            schema.ShouldNotBeNull();
            _schemaResolver.Schemas.ShouldNotBeNull();
            ResolverShouldHaveValidFooSchema();

            var barSchema = _schemaResolver.Schemas.FirstOrDefault(sh => sh.Id == "bar");
            barSchema.ShouldNotBeNull();
            barSchema.Properties.Count.ShouldBe(2);
            barSchema.Properties.ContainsKey("name").ShouldBeTrue();
            barSchema.Properties.ContainsKey("cost").ShouldBeTrue();
        }

        private void ResolverShouldHaveValidFooSchema()
        {
            var fooSchema = _schemaResolver.Schemas.FirstOrDefault(sh => sh.Id == "foo");
            fooSchema.ShouldNotBeNull();
            fooSchema.RequiredProperties.Count.ShouldBe(1);
            fooSchema.RequiredProperties.Contains("id").ShouldBeTrue();
            fooSchema.Properties.Count.ShouldBe(5);
            fooSchema.Properties.ContainsKey("id").ShouldBeTrue();
            fooSchema.Properties.ContainsKey("bar").ShouldBeTrue();
            fooSchema.Properties.ContainsKey("fooType").ShouldBeTrue();
            fooSchema.Properties.ContainsKey("hello").ShouldBeTrue();
            fooSchema.Properties.ContainsKey("world").ShouldBeTrue();
        }

        [Fact]
        public void GenerateSchema_GenerateSchemaFromTypeWithFields_GeneratesSchemaCorrectly()
        {
            var type = typeof(Book);

            var schema = _schemaGenerator.Generate(type, _schemaResolver);

            schema.ShouldNotBeNull();
            _schemaResolver.Schemas.ShouldNotBeNull();
            var bookSchema = _schemaResolver.Schemas.FirstOrDefault(sh => sh.Id == "book");
            bookSchema.ShouldNotBeNull();
            bookSchema.Properties.Count.ShouldBe(4);

            ResolverShouldHaveValidFooSchema();
        }
    }

    public class Foo
    {
        [Required]
        public Guid Id { get; set; }

        [JsonIgnore]
        public string Ignore { get; set; }

        public Bar Bar { get; set; }

        [JsonProperty("hello")]
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