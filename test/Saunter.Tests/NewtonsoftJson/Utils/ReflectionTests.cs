using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Saunter.NewtonsoftJson;
using Saunter.Utils;
using Shouldly;
using Xunit;

namespace Saunter.Tests.NewtonsoftJson.Utils
{
    public class ReflectionTests
    {
        [JsonConverter(typeof(StringEnumConverter))]
        private enum StringEnumConverterEnum
        {
            Hello,
            [EnumMember(Value = "world")]
            World,
        }

        [Theory]
        [InlineData(typeof(StringEnumConverterEnum), new[] { "Hello", "world" })]
        public void IsEnum_True_WhenTypeIsJsonStringEnumConverterEnum(Type type, string[] members)
        {
            var options = new AsyncApiOptions();
            options.UseNewtonsoftJson();
            type.IsEnum(options, out var actualMembers).ShouldBeTrue();
            actualMembers.MemberType.ShouldBe(typeof(string));
            actualMembers.Members.ShouldBe(members);
        }

        private enum SomeEnum
        {
            Hello = 0,
            World = 1,
        }

        [Theory]
        [InlineData(typeof(SomeEnum), new[] { 0, 1 })]
        public void IsEnum_True_WhenTypeIsEnum(Type type, int[] members)
        {
            var options = new AsyncApiOptions();
            options.UseNewtonsoftJson();
            type.IsEnum(options, out var actualMembers).ShouldBeTrue();
            actualMembers.MemberType.ShouldBe(typeof(int));
            actualMembers.Members.ShouldBe(members);
        }
    }
}
