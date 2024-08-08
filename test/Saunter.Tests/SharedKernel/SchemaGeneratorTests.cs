using System;
using System.Linq;
using LEGO.AsyncAPI.Models;
using Saunter.SharedKernel;
using Shouldly;
using Xunit;

namespace Saunter.Tests.SharedKernel
{
    public class SchemaGeneratorTests
    {
        [Theory]
        [InlineData(typeof(bool), "boolean", SchemaType.Boolean, false)]
        [InlineData(typeof(byte), "byte", SchemaType.Integer, false)]
        [InlineData(typeof(short), "int16", SchemaType.Integer, false)]
        [InlineData(typeof(ushort), "uInt16", SchemaType.Integer, false)]
        [InlineData(typeof(int), "int32", SchemaType.Integer, false)]
        [InlineData(typeof(uint), "uInt32", SchemaType.Integer, false)]
        [InlineData(typeof(long), "int64", SchemaType.Integer, false)]
        [InlineData(typeof(ulong), "uInt64", SchemaType.Integer, false)]
        [InlineData(typeof(decimal), "decimal", SchemaType.Number, false)]
        [InlineData(typeof(float), "single", SchemaType.Number, false)]
        [InlineData(typeof(double), "double", SchemaType.Number, false)]
        [InlineData(typeof(bool?), "boolean", SchemaType.Boolean, true)]
        [InlineData(typeof(byte?), "byte", SchemaType.Integer, true)]
        [InlineData(typeof(short?), "int16", SchemaType.Integer, true)]
        [InlineData(typeof(ushort?), "uInt16", SchemaType.Integer, true)]
        [InlineData(typeof(int?), "int32", SchemaType.Integer, true)]
        [InlineData(typeof(uint?), "uInt32", SchemaType.Integer, true)]
        [InlineData(typeof(long?), "int64", SchemaType.Integer, true)]
        [InlineData(typeof(ulong?), "uInt64", SchemaType.Integer, true)]
        [InlineData(typeof(decimal?), "decimal", SchemaType.Number, true)]
        [InlineData(typeof(float?), "single", SchemaType.Number, true)]
        [InlineData(typeof(double?), "double", SchemaType.Number, true)]
        [InlineData(typeof(string), "string", SchemaType.String, true)]
        [InlineData(typeof(DateTime), "dateTime", SchemaType.String, false)]
        [InlineData(typeof(DateTimeOffset), "dateTimeOffset", SchemaType.String, false)]
        [InlineData(typeof(DateOnly), "dateOnly", SchemaType.String, false)]
        [InlineData(typeof(TimeOnly), "timeOnly", SchemaType.String, false)]
        [InlineData(typeof(TimeSpan), "timeSpan", SchemaType.String, false)]
        [InlineData(typeof(Guid), "guid", SchemaType.String, false)]
        [InlineData(typeof(DateTime?), "dateTime", SchemaType.String, true)]
        [InlineData(typeof(DateTimeOffset?), "dateTimeOffset", SchemaType.String, true)]
        [InlineData(typeof(DateOnly?), "dateOnly", SchemaType.String, true)]
        [InlineData(typeof(TimeOnly?), "timeOnly", SchemaType.String, true)]
        [InlineData(typeof(TimeSpan?), "timeSpan", SchemaType.String, true)]
        [InlineData(typeof(Guid?), "guid", SchemaType.String, true)]
        [InlineData(typeof(Uri), "uri", SchemaType.String, true)]
        [InlineData(typeof(object), null, SchemaType.Object, true)]
        [InlineData(typeof(int[]), null, SchemaType.Array, true)]
        [InlineData(typeof(object[]), null, SchemaType.Array, true)]
        public void AsyncApiSchemaGenerator_OnGeneratePrimitive_SchemaTypeAndNameIsMatch(Type type, string format, SchemaType schemaType, bool nullable)
        {
            // Arrange
            AsyncApiSchemaGenerator generator = new();

            // Act
            var schema = generator.Generate(type);

            // Assert
            schema.ShouldNotBeNull();
            schema.Value.All.Count.ShouldBe(1);
            schema.Value.Root.Properties.ShouldBeEmpty();
            schema.Value.Root.Format.ShouldBe(format);
            schema.Value.Root.Type.ShouldBe(schemaType);
            schema.Value.Root.Nullable.ShouldBe(nullable);
        }

        [Fact]
        public void AsyncApiSchemaGenerator_OnGenerateParams_SchemaIsMatch()
        {
            // Arrange
            AsyncApiSchemaGenerator generator = new();
            var type = typeof(Foo);

            // Act
            var schema = generator.Generate(type);

            // Assert
            schema.ShouldNotBeNull();
            schema.Value.All.Count.ShouldBe(8);
            schema.Value.Root.Properties.Count.ShouldBe(7);

            schema.Value.Root.Properties.ShouldContainKey("id");
            var id = schema.Value.Root.Properties["id"];
            id.Type.ShouldBe(SchemaType.String);
            id.Format.ShouldBe("guid");
            id.Title.ShouldBe("guid");
            id.Nullable.ShouldBeFalse();

            schema.Value.Root.Properties.ShouldContainKey("myUri");
            var myUri = schema.Value.Root.Properties["myUri"];
            myUri.Type.ShouldBe(SchemaType.String);
            myUri.Format.ShouldBe("uri");
            myUri.Title.ShouldBe("uri");
            myUri.Nullable.ShouldBeTrue();

            schema.Value.Root.Properties.ShouldContainKey("bar");
            var bar = schema.Value.Root.Properties["bar"];
            bar.Type.ShouldBe(SchemaType.Object);
            bar.Title.ShouldBe("bar");
            bar.Format.ShouldBeNull();

            bar.Properties.ShouldContainKey("name");
            var barName = bar.Properties["name"];
            barName.Type.ShouldBe(SchemaType.String);
            barName.Title.ShouldBe("string");
            barName.Format.ShouldBe("string");
            barName.Nullable.ShouldBeTrue();

            bar.Properties.ShouldContainKey("cost");
            var barCost = bar.Properties["cost"];
            barCost.Type.ShouldBe(SchemaType.Number);
            barCost.Title.ShouldBe("decimal");
            barCost.Format.ShouldBe("decimal");
            barCost.Nullable.ShouldBeTrue();

            schema.Value.Root.Properties.ShouldContainKey("helloWorld");
            var helloWorld = schema.Value.Root.Properties["helloWorld"];
            helloWorld.Type.ShouldBe(SchemaType.String);
            helloWorld.Title.ShouldBe("string");
            helloWorld.Format.ShouldBe("string");
            helloWorld.Nullable.ShouldBeTrue();

            schema.Value.Root.Properties.ShouldContainKey("helloWorld2");
            var helloWorld2 = schema.Value.Root.Properties["helloWorld2"];
            helloWorld2.Type.ShouldBe(SchemaType.String);
            helloWorld2.Title.ShouldBe("string");
            helloWorld2.Format.ShouldBe("string");
            helloWorld2.Nullable.ShouldBeTrue();

            schema.Value.Root.Properties.ShouldContainKey("timestamp");
            var timestamp = schema.Value.Root.Properties["timestamp"];
            timestamp.Type.ShouldBe(SchemaType.String);
            timestamp.Title.ShouldBe("dateTimeOffset");
            timestamp.Format.ShouldBe("dateTimeOffset");
            timestamp.Nullable.ShouldBeFalse();

            schema.Value.Root.Properties.ShouldContainKey("fooType");
            var fooType = schema.Value.Root.Properties["fooType"];
            fooType.Type.ShouldBe(SchemaType.String);
            fooType.Title.ShouldBe("fooType");
            fooType.Format.ShouldBe("enum");
            fooType.Nullable.ShouldBeFalse();

            fooType.Enum
                .Select(s => s.GetValue<string>())
                .SequenceEqual(Enum.GetNames<FooType>())
                .ShouldBeTrue();
        }

        [Fact]
        public void AsyncApiSchemaGenerator_OnLoopGenerate_NotFailed()
        {
            // Arrange
            AsyncApiSchemaGenerator generator = new();
            var type = typeof(Loop);

            // Act
            var schema = generator.Generate(type);

            // Assert
            schema.ShouldNotBeNull();

            schema.Value.All.Count.ShouldBe(1);
            schema.Value.Root.Properties.Count.ShouldBe(2);

            schema.Value.Root.Properties.ShouldContainKey("ultraLoop");
            schema.Value.Root.Properties.ShouldContainKey("ultraLoop2");

            var loop = schema.Value.Root.Properties["ultraLoop"];
            loop.Reference.ShouldNotBeNull();
            loop.Reference.Id.ShouldBe("loop");
            loop.Reference.Type.ShouldBe(ReferenceType.Schema);

            var loop2 = schema.Value.Root.Properties["ultraLoop2"];
            loop2.Reference.ShouldNotBeNull();
            loop2.Reference.Id.ShouldBe("loop");
            loop2.Reference.Type.ShouldBe(ReferenceType.Schema);
        }
    }

    public class Foo
    {
        public Guid Id { get; set; }
        public Uri MyUri { get; set; }
        public Bar Bar { get; set; }
        public string HelloWorld { get; set; }
        public string HelloWorld2 { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public FooType FooType { get; set; }
    }

    public enum FooType { Foo, Bar }

    public class Bar
    {
        public string Name { get; set; }
        public decimal? Cost { get; set; }
    }

    public class Loop
    {
        public Loop UltraLoop { get; set; }
        public Loop UltraLoop2 { get; set; }
    }
}
