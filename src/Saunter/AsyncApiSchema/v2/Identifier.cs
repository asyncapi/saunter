﻿using System;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2
{
    [JsonConverter(typeof(IdentifierConverter))]
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

        public class IdentifierConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(Identifier);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType != JsonToken.String) return null;

                var text = reader.ReadAsString();
                return new Identifier(text);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }
        }
    }
}