using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Saunter.Utils;

namespace Saunter.NewtonsoftJson
{
    public static class SaunterNewtonsoftJsonServiceCollectionExtensions
    {
        public static AsyncApiOptions UseNewtonsoftJson(this AsyncApiOptions options)
        {
            options.PropertyNameSelector = prop =>
            {
                var name = prop.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
                    ?? prop.GetCustomAttribute<DataMemberAttribute>()?.Name
                    ?? prop.Name;
                return new CamelCaseNamingStrategy().GetPropertyName(name, false);
            };

            options.PropertyFilter = prop => prop.GetCustomAttribute<JsonIgnoreAttribute>() == null;

            options.UseEnumMemberName = type =>
            {
                var jsonConverterAttribute = type.GetCustomAttribute<JsonConverterAttribute>();
                return jsonConverterAttribute?.ConverterType == typeof(StringEnumConverter);
            };

            options.EnumMemberNameSelector = (type, val) =>
            {
                var converterType = type.GetCustomAttribute<JsonConverterAttribute>()?.ConverterType;
                if (converterType == typeof(StringEnumConverter))
                {
                    var enumMemberAttribute = val.GetCustomAttribute<EnumMemberAttribute>();
                    if (enumMemberAttribute?.Value != null)
                    {
                        return enumMemberAttribute.Value;
                    }
                }

                return val.ToString();
            };

            return options;
        }
    }
}
