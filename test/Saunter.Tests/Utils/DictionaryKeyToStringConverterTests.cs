using System;
using System.Collections.Generic;
using System.Text.Json;
using Saunter.AsyncApiSchema.v2;
using Saunter.Utils;
using Shouldly;
using Xunit;

namespace Saunter.Tests.Utils
{
    public class DictionaryKeyToStringConverterTests
    {
        [Theory]
        [InlineData(typeof(IDictionary<ComponentFieldName, IChannelBinding>))]
        [InlineData(typeof(Dictionary<ComponentFieldName, IChannelBinding>))]
        public void CanConvert_True_OnConvertibleTypes(Type type)
        {
            var converter = new DictionaryKeyToStringConverter();
            converter.CanConvert(type).ShouldBeTrue();
        }

        [Fact]
        public void Convert_Should_OutputCorrectData()
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
    }
}
