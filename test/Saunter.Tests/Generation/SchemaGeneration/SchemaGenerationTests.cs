using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NJsonSchema.Generation;
using Saunter.AsyncApiSchema.v2;
using Saunter.Attributes;
using Saunter.Generation.SchemaGeneration;
using Shouldly;
using Xunit;

namespace Saunter.Tests.Generation.SchemaGeneration
{
    public class SchemaGenerationTests
    {
        private readonly AsyncApiSchemaResolver _schemaResolver;
        private readonly JsonSchemaGenerator _schemaGenerator;

        public SchemaGenerationTests()
        {
            var settings = new JsonSchemaGeneratorSettings()
            {
                TypeNameGenerator = new CamelCaseTypeNameGenerator(),
                SerializerSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };
            _schemaResolver = new AsyncApiSchemaResolver(new AsyncApiDocument(), settings);
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
            fooSchema.RequiredProperties.Count.ShouldBe(2);
            fooSchema.RequiredProperties.Contains("id").ShouldBeTrue();
            fooSchema.RequiredProperties.Contains("bar").ShouldBeTrue();
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

        [Fact]
        public void GenerateSchema_GenerateSchemaFromClassWithDiscriminator_GeneratesSchemaCorrectly()
        {
            var type = typeof(Pet);

            var schema = _schemaGenerator.Generate(type, _schemaResolver);

            schema.ShouldNotBeNull();
            _schemaResolver.Schemas.ShouldNotBeNull();
            var petSchema = _schemaResolver.Schemas.FirstOrDefault(s => s.Id == "pet");
            petSchema.Discriminator.ShouldBe("petType");
            petSchema.OneOf.Count().ShouldBe(2);

            var catSchema = _schemaResolver.Schemas.FirstOrDefault(s => s.Id == "cat");
            catSchema.Properties.Count.ShouldBe(3);
            catSchema.Properties.ContainsKey("petType").ShouldBeTrue();
            catSchema.Properties.ContainsKey("name").ShouldBeTrue();
            catSchema.Properties.ContainsKey("huntingSkill").ShouldBeTrue();

            var dogSchema = _schemaResolver.Schemas.FirstOrDefault(s => s.Id == "dog");
            dogSchema.Properties.Count.ShouldBe(3);
            dogSchema.Properties.ContainsKey("petType").ShouldBeTrue();
            dogSchema.Properties.ContainsKey("name").ShouldBeTrue();
            dogSchema.Properties.ContainsKey("packSize").ShouldBeTrue();
        }

        [Fact]
        public void GenerateSchema_GenerateSchemaFromInterfaceWithDiscriminator_GeneratesSchemaCorrectly()
        {
            var type = typeof(IPet);

            var schema = _schemaGenerator.Generate(type, _schemaResolver);

            schema.ShouldNotBeNull();
            _schemaResolver.Schemas.ShouldNotBeNull();
            var ipetSchema = _schemaResolver.Schemas.FirstOrDefault(s => s.Id == "iPet");
            ipetSchema.Discriminator.ShouldBe("petType");
            ipetSchema.OneOf.Count().ShouldBe(2);

            var catSchema = _schemaResolver.Schemas.FirstOrDefault(s => s.Id == "cat");
            catSchema.Properties.Count.ShouldBe(3);
            catSchema.Properties.ContainsKey("petType").ShouldBeTrue();
            catSchema.Properties.ContainsKey("name").ShouldBeTrue();
            catSchema.Properties.ContainsKey("huntingSkill").ShouldBeTrue();

            var dogSchema = _schemaResolver.Schemas.FirstOrDefault(s => s.Id == "dog");
            dogSchema.Properties.Count.ShouldBe(3);
            dogSchema.Properties.ContainsKey("petType").ShouldBeTrue();
            dogSchema.Properties.ContainsKey("name").ShouldBeTrue();
            dogSchema.Properties.ContainsKey("packSize").ShouldBeTrue();
        }
    }

    public class Foo
    {
        [Required]
        public Guid Id { get; set; }

        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public string Ignore { get; set; }

        [Required]
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

    [Discriminator("petType")]
    [DiscriminatorSubType(typeof(Cat))]
    [DiscriminatorSubType(typeof(Dog))]
    public interface IPet
    {
        string PetType { get; }

        string Name { get; }
    }

    [Discriminator("petType")]
    [DiscriminatorSubType(typeof(Cat))]
    [DiscriminatorSubType(typeof(Dog))]
    public abstract class Pet : IPet
    {
        public string PetType { get; set; }

        public string Name { get; set; }
    }

    public class Cat : Pet
    {
        public string HuntingSkill { get; set; }
    }

    public class Dog : Pet
    {
        public string PackSize { get; set; }
    }
}