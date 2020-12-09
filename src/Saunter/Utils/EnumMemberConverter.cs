using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.Utils
{
    public class EnumMemberConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Type converterType = typeof(EnumMemberConverterInner<>).MakeGenericType(typeToConvert);
            return (JsonConverter?)Activator.CreateInstance(converterType);
        }

        private class EnumMemberConverterInner<TEnum> : JsonConverter<TEnum>
            where TEnum : Enum
        {
            public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                // Doesn't support deserialization
                throw new NotSupportedException();
            }

            public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
            {
                if (TryGetEnumMemberValue(value, out string? name))
                    JsonSerializer.Serialize(writer, name, options);
                else
                    JsonSerializer.Serialize(writer, value.ToString(), options);
            }

            private static bool TryGetEnumMemberValue(TEnum value, out string? name)
            {
                MemberInfo memberInfo = typeof(TEnum).GetMember(value.ToString()).First();
                EnumMemberAttribute? enumMemberAttribute = memberInfo.GetCustomAttribute<EnumMemberAttribute>();

                if (enumMemberAttribute != null)
                {
                    name = enumMemberAttribute.Value;
                    return true;
                }

                name = null;
                return false;
            }
        }
    }
}
