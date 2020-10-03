using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.Utils
{
    public class DictionaryKeyToStringConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
                return false;

            // Check whether type either IS an IDictionary or implements it
            return typeToConvert.GetGenericTypeDefinition() == typeof(IDictionary<,>) ||
                   typeToConvert.GetInterface("IDictionary") != null;
        }

        public override JsonConverter CreateConverter(
            Type type,
            JsonSerializerOptions options)
        {
            Type keyType = type.GetGenericArguments()[0];
            Type valueType = type.GetGenericArguments()[1];

            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                typeof(DictionaryKeyToStringConverterInner<,>).MakeGenericType(
                    new Type[] { keyType, valueType }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { options },
                culture: null);

            return converter;
        }

        private class DictionaryKeyToStringConverterInner<TKey, TValue> :
            JsonConverter<IDictionary<TKey, TValue>>
        {
            private readonly JsonConverter<TValue> _valueConverter;
            private readonly Type _valueType;

            public DictionaryKeyToStringConverterInner(JsonSerializerOptions options)
            {
                // For performance, use the existing converter if available.
                _valueConverter = (JsonConverter<TValue>)options
                    .GetConverter(typeof(TValue));

                // Cache the key and value types.
                _valueType = typeof(TValue);
            }

            public override IDictionary<TKey, TValue> Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options)
            {
                // Doesn't support deserialization
                throw new NotSupportedException();
            }

            public override void Write(
                Utf8JsonWriter writer,
                IDictionary<TKey, TValue> dictionary,
                JsonSerializerOptions options)
            {
                writer.WriteStartObject();

                foreach (KeyValuePair<TKey, TValue> kvp in dictionary)
                {
                    writer.WritePropertyName(kvp.Key.ToString());

                    if (_valueConverter != null)
                        _valueConverter.Write(writer, kvp.Value, options);
                    else
                        JsonSerializer.Serialize(writer, kvp.Value, options);
                }

                writer.WriteEndObject();
            }
        }
    }
}
