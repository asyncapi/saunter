using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2 
{
    [JsonConverter(typeof(JsonConverter))]
    public class Identifier
    {
        private readonly Uri value;

        public Identifier(string identifier)
        {
            if (identifier == null) throw new ArgumentNullException(nameof(identifier));

            value = new Uri(identifier);
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public class JsonConverter : JsonConverter<Identifier?>
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(Identifier);
            }

            public override Identifier? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.String) return null;

                var text = reader.GetString();
                return new Identifier(text);
            }

            public override void Write(Utf8JsonWriter writer, Identifier? value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value?.ToString());
            }
        }
    }
}