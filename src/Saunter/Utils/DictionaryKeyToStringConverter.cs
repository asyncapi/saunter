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
            // Check whether type either IS an IDictionary or implements it
            return TypeIsDictionary(typeToConvert)
                   || TypeIsDictionary(typeToConvert.BaseType); // e.g. class Servers : Dictionary<ServersFieldName, Server>
        }

        private static bool TypeIsDictionary(Type type)
            => type != null
                   && type.IsGenericType
                   && (type.GetGenericTypeDefinition() == typeof(IDictionary<,>) || type.GetInterface("IDictionary") != null);

        public override JsonConverter CreateConverter(
            Type type,
            JsonSerializerOptions options)
        {
            var genericArguments = TypeIsDictionary(type)
                ? type.GetGenericArguments()
                : type.BaseType?.GetGenericArguments();

            if (genericArguments == null || genericArguments.Length != 2)
            {
                throw new NotSupportedException($"Type {type} not supported by this converted.");
            }

            var keyType = genericArguments[0];
            var valueType = genericArguments[1];


            var converterType = TypeIsDictionary(type)
                ? typeof(DictionaryKeyToStringConverterInner<,>).MakeGenericType(new Type[] { keyType, valueType })
                : typeof(InheritedDictionaryKeyToStringConverterInner<,,>).MakeGenericType(new Type[] { type, keyType, valueType });

            var converter = (JsonConverter)Activator.CreateInstance(
                converterType,
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { options },
                culture: null);

            return converter;
        }

        private class InheritedDictionaryKeyToStringConverterInner<TType, TKey, TValue> :
            JsonConverter<TType> where TType : IDictionary<TKey, TValue>
        {
            private readonly DictionaryKeyToStringConverterInner<TKey, TValue> _innerConverter;

            public InheritedDictionaryKeyToStringConverterInner(JsonSerializerOptions options)
            {
                _innerConverter = new DictionaryKeyToStringConverterInner<TKey, TValue>(options);
            }

            public override TType Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options)
            {
                return (TType)_innerConverter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(
                Utf8JsonWriter writer,
                TType dictionary,
                JsonSerializerOptions options)
            {
                _innerConverter.Write(writer, dictionary, options);
            }
        }

        private class DictionaryKeyToStringConverterInner<TKey, TValue> :
            JsonConverter<IDictionary<TKey, TValue>>
        {
            private readonly JsonConverter<TValue> _valueConverter;

            public DictionaryKeyToStringConverterInner(JsonSerializerOptions options)
            {
                // For performance, use the existing converter if available.
                _valueConverter = (JsonConverter<TValue>)options
                    .GetConverter(typeof(TValue));
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