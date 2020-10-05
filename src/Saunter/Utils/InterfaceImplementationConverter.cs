using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.Utils
{
    public class InterfaceImplementationConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert.IsInterface;

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Type converterType = typeof(InterfaceImplementationConverterInner<>).MakeGenericType(typeToConvert);
            return (JsonConverter)Activator.CreateInstance(converterType);
        }

        private class InterfaceImplementationConverterInner<T> : JsonConverter<T>
        {
            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                // Doesn't support deserialization
                throw new NotSupportedException();
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                // Use the actual type at runtime to serialize the value
                Type actualType = value?.GetType();
                JsonSerializer.Serialize(writer, value, actualType, options);
            }
        }
    }
}
