using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

using Saunter.Attributes;
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

        [Fact]
        public void GenerateSchema_GenerateSchemaFromClassWithDiscriminator_GeneratesSchemaCorrectly()
        {
            var type = typeof(Pet);

            var schema = _schemaGenerator.GenerateSchema(type, _schemaRepository);

            schema.ShouldNotBeNull();
            _schemaRepository.Schemas.ShouldNotBeNull();
            _schemaRepository.Schemas.ContainsKey("pet").ShouldBeTrue();
            _schemaRepository.Schemas["pet"].Discriminator.ShouldBe("petType");
            _schemaRepository.Schemas["pet"].OneOf.Count().ShouldBe(2);
            _schemaRepository.Schemas.ContainsKey("cat").ShouldBeTrue();
            _schemaRepository.Schemas["cat"].Properties.Count.ShouldBe(3);
            _schemaRepository.Schemas["cat"].Properties.ContainsKey("petType").ShouldBeTrue();
            _schemaRepository.Schemas["cat"].Properties.ContainsKey("name").ShouldBeTrue();
            _schemaRepository.Schemas["cat"].Properties.ContainsKey("huntingSkill").ShouldBeTrue();
            _schemaRepository.Schemas.ContainsKey("dog").ShouldBeTrue();
            _schemaRepository.Schemas["dog"].Properties.Count.ShouldBe(3);
            _schemaRepository.Schemas["dog"].Properties.ContainsKey("petType").ShouldBeTrue();
            _schemaRepository.Schemas["dog"].Properties.ContainsKey("name").ShouldBeTrue();
            _schemaRepository.Schemas["dog"].Properties.ContainsKey("packSize").ShouldBeTrue();
        }

        [Fact]
        public void GenerateSchema_GenerateSchemaFromInterfaceWithDiscriminator_GeneratesSchemaCorrectly()
        {
            var type = typeof(IPet);

            var schema = _schemaGenerator.GenerateSchema(type, _schemaRepository);

            schema.ShouldNotBeNull();
            _schemaRepository.Schemas.ShouldNotBeNull();
            _schemaRepository.Schemas.ContainsKey("iPet").ShouldBeTrue();
            _schemaRepository.Schemas["iPet"].Discriminator.ShouldBe("petType");
            _schemaRepository.Schemas["iPet"].OneOf.Count().ShouldBe(2);
            _schemaRepository.Schemas.ContainsKey("cat").ShouldBeTrue();
            _schemaRepository.Schemas["cat"].Properties.Count.ShouldBe(3);
            _schemaRepository.Schemas["cat"].Properties.ContainsKey("petType").ShouldBeTrue();
            _schemaRepository.Schemas["cat"].Properties.ContainsKey("name").ShouldBeTrue();
            _schemaRepository.Schemas["cat"].Properties.ContainsKey("huntingSkill").ShouldBeTrue();
            _schemaRepository.Schemas.ContainsKey("dog").ShouldBeTrue();
            _schemaRepository.Schemas["dog"].Properties.Count.ShouldBe(3);
            _schemaRepository.Schemas["dog"].Properties.ContainsKey("petType").ShouldBeTrue();
            _schemaRepository.Schemas["dog"].Properties.ContainsKey("name").ShouldBeTrue();
            _schemaRepository.Schemas["dog"].Properties.ContainsKey("packSize").ShouldBeTrue();
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