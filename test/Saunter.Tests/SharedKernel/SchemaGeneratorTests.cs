using System;
using LEGO.AsyncAPI.Models;
using Saunter.SharedKernel;
using Xunit;

namespace Saunter.Tests.SharedKernel
{
    public class SchemaGeneratorTests
    {
        [Theory]
        [InlineData(typeof(bool), "boolean", SchemaType.Boolean)]
        [InlineData(typeof(byte), "byte", SchemaType.Integer)]
        [InlineData(typeof(short), "int16", SchemaType.Integer)]
        [InlineData(typeof(ushort), "uInt16", SchemaType.Integer)]
        [InlineData(typeof(int), "int32", SchemaType.Integer)]
        [InlineData(typeof(uint), "uInt32", SchemaType.Integer)]
        [InlineData(typeof(long), "int64", SchemaType.Integer)]
        [InlineData(typeof(ulong), "uInt64", SchemaType.Integer)]
        [InlineData(typeof(decimal), "decimal", SchemaType.Number)]
        [InlineData(typeof(float), "single", SchemaType.Number)]
        [InlineData(typeof(double), "double", SchemaType.Number)]
        [InlineData(typeof(string), "string", SchemaType.String)]
        [InlineData(typeof(DateTime), "dateTime", SchemaType.String)]
        [InlineData(typeof(DateTimeOffset), "dateTimeOffset", SchemaType.String)]
        [InlineData(typeof(DateOnly), "dateOnly", SchemaType.String)]
        [InlineData(typeof(TimeOnly), "timeOnly", SchemaType.String)]
        [InlineData(typeof(TimeSpan), "timeSpan", SchemaType.String)]
        [InlineData(typeof(Guid), "guid", SchemaType.String)]
        [InlineData(typeof(Uri), "uri", SchemaType.String)]
        [InlineData(typeof(object), "object", SchemaType.Object)]
        [InlineData(typeof(int[]), "int32[]", SchemaType.Array)]
        [InlineData(typeof(object[]), "object[]", SchemaType.Array)]
        public void AsyncApiSchemaGenerator_OnGeneratePrimitive_SchemaTypeAndNameIsMatch(Type type, string title, SchemaType schemaType)
        {
            // Arrange
            AsyncApiSchemaGenerator generator = new();

            // Act
            var schema = generator.Generate(type);

            // Assert
            Assert.NotNull(schema);
            Assert.Empty(schema.Properties);
            Assert.Equal(title, schema.Title);
            Assert.Equal(schemaType, schema.Type);
        }
    }
}
