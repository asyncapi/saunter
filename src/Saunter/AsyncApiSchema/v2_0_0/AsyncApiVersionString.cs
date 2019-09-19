using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2_0_0 {
    public class AsyncApiVersionString
    {
        private readonly string value;

        public AsyncApiVersionString(string asyncapi)
        {
            value = EnsureSupportedVersion(asyncapi);
        }

        public override string ToString()
        {
            return value;
        }

        public static readonly IEnumerable<string> Versions = new[] { "2.0.0" };

        public static string EnsureSupportedVersion(string asyncapi)
        {
            return Versions.Contains(asyncapi, StringComparer.OrdinalIgnoreCase)
                ? asyncapi
                : throw new NotSupportedException($"asyncapi version '{asyncapi}' is not supported");
        }

        public class JsonConverter : Newtonsoft.Json.JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(AsyncApiVersionString);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType != JsonToken.String) return null;

                var text = reader.Value.ToString();
                return new AsyncApiVersionString(text);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var asyncapi = value as AsyncApiVersionString;
                serializer.Serialize(writer, asyncapi.ToString());
            }
        }
    }
}