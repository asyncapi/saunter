using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using NJsonSchema;
using NJsonSchema.Converters;
using NJsonSchema.Generation;

using Saunter.AsyncApiSchema.v2;
using Saunter.Generation.SchemaGeneration;
using Saunter.Tests.Utils;

using Shouldly;

using Xunit;

namespace Saunter.Tests.Generation.SchemaGeneration;

public class SchemaGenerationTests
{
    private readonly AsyncApiSchemaResolver _schemaResolver;
    private readonly JsonSchemaGenerator _schemaGenerator;

    public SchemaGenerationTests()
    {
        AsyncApiSchemaOptions settings = new()
        {
            TypeNameGenerator = new CamelCaseTypeNameGenerator(),
            SerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            },
        };
        _schemaResolver = new AsyncApiSchemaResolver(new AsyncApiDocument(), settings);
        _schemaGenerator = new JsonSchemaGenerator(settings);
    }

    [Fact]
    public void GenerateSchema_GenerateSchemaFromTypeWithProperties_GeneratesSchemaCorrectly()
    {
        Type type = typeof(Foo);

        JsonSchema schema = _schemaGenerator.Generate(type, _schemaResolver);

        schema.ShouldNotBeNull();
        _schemaResolver.Schemas.ShouldNotBeNull();
        ResolverShouldHaveValidFooSchema();

        JsonSchema barSchema = _schemaResolver.Schemas.FirstOrDefault(sh => sh.Id == "bar");
        barSchema.ShouldNotBeNull();
        barSchema.Properties.Count.ShouldBe(2);
        barSchema.Properties.ContainsKey("name").ShouldBeTrue();
        barSchema.Properties.ContainsKey("cost").ShouldBeTrue();
    }

    private void ResolverShouldHaveValidFooSchema()
    {
        JsonSchema fooSchema = _schemaResolver.Schemas.FirstOrDefault(sh => sh.Id == "foo");
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
        Type type = typeof(Book);

        JsonSchema schema = _schemaGenerator.Generate(type, _schemaResolver);

        schema.ShouldNotBeNull();
        _schemaResolver.Schemas.ShouldNotBeNull();
        JsonSchema bookSchema = _schemaResolver.Schemas.FirstOrDefault(sh => sh.Id == "book");
        bookSchema.ShouldNotBeNull();
        bookSchema.Properties.Count.ShouldBe(4);

        ResolverShouldHaveValidFooSchema();
    }

    [Fact]
    public void GenerateSchema_GenerateSchemaFromClassWithDiscriminator_GeneratesSchemaCorrectly()
    {
        Type type = typeof(PetOwner);

        JsonSchema schema = _schemaGenerator.Generate(type, _schemaResolver);

        schema.ShouldNotBeNull();

        _schemaResolver.Schemas.ShouldNotBeNull();
        JsonSchema petSchema = _schemaResolver.Schemas.FirstOrDefault(s => s.Id == "pet");
        petSchema.Discriminator.ShouldBe("petType");

        schema.Properties["pet"].IsNullable(SchemaType.JsonSchema).ShouldBeTrue();

        JsonSchema catSchema = _schemaResolver.Schemas.FirstOrDefault(s => s.Id == "cat");
        System.Collections.Generic.IDictionary<string, JsonSchema> catProperties = catSchema.MergeAllProperties();
        catProperties.Count.ShouldBe(3);
        catProperties.ContainsKey("petType").ShouldBeTrue();
        catProperties.ContainsKey("name").ShouldBeTrue();
        catProperties.ContainsKey("huntingSkill").ShouldBeTrue();

        JsonSchema dogSchema = _schemaResolver.Schemas.FirstOrDefault(s => s.Id == "dog");
        System.Collections.Generic.IDictionary<string, JsonSchema> dogProperties = dogSchema.MergeAllProperties();
        dogProperties.Count.ShouldBe(3);
        dogProperties.ContainsKey("petType").ShouldBeTrue();
        dogProperties.ContainsKey("name").ShouldBeTrue();
        dogProperties.ContainsKey("packSize").ShouldBeTrue();
    }

    [Fact()]
    public void GenerateSchema_GenerateSchemaFromInterfaceWithDiscriminator_GeneratesSchemaCorrectly()
    {
        Type type = typeof(IPetOwner);

        JsonSchema schema = _schemaGenerator.Generate(type, _schemaResolver);

        schema.ShouldNotBeNull();
        _schemaResolver.Schemas.ShouldNotBeNull();
        JsonSchema ipetSchema = _schemaResolver.Schemas.FirstOrDefault(s => s.Id == "iPet");
        ipetSchema.Discriminator.ShouldBe("petType");

        schema.Properties["pet"].IsNullable(SchemaType.JsonSchema).ShouldBeTrue();

        JsonSchema catSchema = _schemaResolver.Schemas.FirstOrDefault(s => s.Id == "cat");
        System.Collections.Generic.IDictionary<string, JsonSchema> catProperties = catSchema.MergeAllProperties();
        catProperties.Count.ShouldBe(3);
        catProperties.ContainsKey("petType").ShouldBeTrue();
        catProperties.ContainsKey("name").ShouldBeTrue();
        catProperties.ContainsKey("huntingSkill").ShouldBeTrue();

        JsonSchema dogSchema = _schemaResolver.Schemas.FirstOrDefault(s => s.Id == "dog");
        System.Collections.Generic.IDictionary<string, JsonSchema> dogProperties = dogSchema.MergeAllProperties();
        dogProperties.Count.ShouldBe(3);
        dogProperties.ContainsKey("petType").ShouldBeTrue();
        dogProperties.ContainsKey("name").ShouldBeTrue();
        dogProperties.ContainsKey("packSize").ShouldBeTrue();
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

public class PetOwner
{
    public Pet Pet { get; set; }
}

public class IPetOwner
{
    public IPet Pet { get; set; }
}

[JsonConverter(typeof(JsonInheritanceConverter), "petType")]
[JsonInheritance("cat", typeof(Cat))]
[JsonInheritance("dog", typeof(Dog))]
public interface IPet
{
    string PetType { get; }

    string Name { get; }
}

[JsonConverter(typeof(JsonInheritanceConverter), "petType")]
[KnownType(typeof(Cat))]
[KnownType(typeof(Dog))]
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