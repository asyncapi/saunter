using System;
using System.Collections.Generic;
using System.Text.Json;
using Saunter.AsyncApiSchema.v2;
using Saunter.Utils;
using Shouldly;
using Xunit;

namespace Saunter.Tests.Utils
{
    public class JsonConverterTests
    {
        [Theory]
        [InlineData(typeof(IDictionary<ComponentFieldName, IChannelBinding>))]
        [InlineData(typeof(Dictionary<ComponentFieldName, IChannelBinding>))]
        public void DictionaryKeyToStringConverter_CanConvert_True_OnConvertibleTypes(Type type)
        {
            var converter = new DictionaryKeyToStringConverter();
            converter.CanConvert(type).ShouldBeTrue();
        }

        [Fact]
        public void DictionaryKeyToStringConverter_Convert_Should_OutputCorrectData()
        {
            // Check whether serialization works
            var dict = new Dictionary<ComponentFieldName, int>
            {
                { new ComponentFieldName("test1"), 1 },
                { new ComponentFieldName("test2"), 2 },
            };

            var data = JsonSerializer.Serialize(
                dict,
                new JsonSerializerOptions
                {
                    WriteIndented = false,
                    Converters = { new DictionaryKeyToStringConverter() }
                }
            );

            data.ShouldBe("{\"test1\":1,\"test2\":2}");
        }

        [Theory]
        [InlineData(AsyncApiVersionString.v2, "\"2.0.0\"")]
        [InlineData(SecuritySchemeType.X509, "\"X509\"")]
        public void JsonSerializer_Should_RespectEnumMemberAttribute(Enum value, string expectedValue)
        {
            string actualValue = JsonSerializer.Serialize(value, value.GetType());
            actualValue.ShouldBe(expectedValue);
        }
    }
}
