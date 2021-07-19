using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Saunter.Utils;
using Shouldly;
using Xunit;

namespace Saunter.Tests.Utils
{
    public class ReflectionTests
    {
        private class ExampleForTestingAttribute : Attribute { }

        [ExampleForTesting]
        private class TypeWithAttribute { }

        [Fact]
        public void HasCustomAttribute_True_WhenTypeHasCustomAttribute()
        {
            var type = typeof(TypeWithAttribute).GetTypeInfo();

            type.HasCustomAttribute<ExampleForTestingAttribute>().ShouldBeTrue();
        }

        private class TypeWithoutAttribute { }

        [Fact]
        public void HasCustomAttribute_False_WhenTypeDoesNotHaveCustomAttribute()
        {
            var type = typeof(TypeWithoutAttribute).GetTypeInfo();

            type.HasCustomAttribute<ExampleForTestingAttribute>().ShouldBeFalse();
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(int?))]
        [InlineData(typeof(uint))]
        [InlineData(typeof(uint?))]
        [InlineData(typeof(long))]
        [InlineData(typeof(long?))]
        [InlineData(typeof(ulong))]
        [InlineData(typeof(ulong?))]
        [InlineData(typeof(short))]
        [InlineData(typeof(short?))]
        [InlineData(typeof(ushort))]
        [InlineData(typeof(ushort?))]
        public void IsInteger_True_WhenTypeIsSimilarToInteger(Type type)
        {
            type.IsInteger().ShouldBeTrue();
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(decimal))]
        public void IsInteger_False_WhenTypeIsNotSimilarToInteger(Type type)
        {
            type.IsInteger().ShouldBeFalse();
        }

        [Theory]
        [InlineData(typeof(float))]
        [InlineData(typeof(float?))]
        [InlineData(typeof(double))]
        [InlineData(typeof(double?))]
        [InlineData(typeof(decimal))]
        [InlineData(typeof(decimal?))]
        public void IsNumber_True_WhenTypeIsSimilarToNumber(Type type)
        {
            type.IsNumber().ShouldBeTrue();
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        public void IsNumber_False_WhenTypeIsNotSimilarToNumber(Type type)
        {
            type.IsNumber().ShouldBeFalse();
        }

        [Theory]
        [InlineData(typeof(bool))]
        [InlineData(typeof(bool?))]
        public void IsBoolean_True_WhenTypeIsBoolean(Type type)
        {
            type.IsBoolean().ShouldBeTrue();
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(byte))]
        public void IsBoolean_False_WhenTypeIsNotBoolean(Type type)
        {
            type.IsBoolean().ShouldBeFalse();
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        public void IsGuid_False_WhenTypeIsNotGuid(Type type)
        {
            type.IsGuid().ShouldBeFalse();
        }

        [Theory]
        [InlineData(typeof(Guid))]
        [InlineData(typeof(Guid?))]
        public void IsGuid_True_WhenTypeIsGuid(Type type)
        {
            type.IsGuid().ShouldBeTrue();
        }

        private class CustomEnumerableType : IEnumerable<string>
        {
            public IEnumerator<string> GetEnumerator()
            {
                foreach (var s in new[] {"Hello", "World"})
                {
                    yield return s;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [InlineData(typeof(IEnumerable<int>), typeof(int))]
        [InlineData(typeof(int[]), typeof(int))]
        [InlineData(typeof(string[]), typeof(string))]
        [InlineData(typeof(List<int>), typeof(int))]
        [InlineData(typeof(HashSet<int>), typeof(int))]
        [InlineData(typeof(CustomEnumerableType), typeof(string))]
        public void IsEnumerable_True_WhenTypeIsEnumerable(Type type, Type elementType)
        {
            type.IsEnumerable(out var actualElementType).ShouldBeTrue();
            actualElementType.ShouldBe(elementType);
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(string))]
        public void IsEnumerable_False_WhenTypeIsNotEnumerable(Type type)
        {
            type.IsEnumerable(out _).ShouldBeFalse();
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        private enum JsonStringEnumConverterEnum
        {
            Hello,
            [EnumMember(Value = "world")]
            World,
        }

        [Theory]
        [InlineData(typeof(JsonStringEnumConverterEnum), new[] { "Hello", "World" })]
        public void IsEnum_True_WhenTypeIsJsonStringEnumConverterEnum(Type type, string[] members)
        {
            var options = new AsyncApiOptions();
            type.IsEnum(options, out var actualMembers).ShouldBeTrue();
            actualMembers.MemberType.ShouldBe(typeof(string));
            actualMembers.Members.ShouldBe(members);
        }
        
        [JsonConverter(typeof(JsonStringEnumMemberConverter))]
        private enum JsonStringEnumMemberConverterEnum
        {
            Hello,
            [EnumMember(Value = "world")]
            World,
        }

        [Theory]
        [InlineData(typeof(JsonStringEnumMemberConverterEnum), new[] { "Hello", "world" })]
        public void IsEnum_True_WhenTypeIsJsonStringEnumMemberConverterEnum(Type type, string[] members)
        {
            var options = new AsyncApiOptions();
            type.IsEnum(options, out var actualMembers).ShouldBeTrue();
            actualMembers.MemberType.ShouldBe(typeof(string));
            actualMembers.Members.ShouldBe(members);
        }

        [JsonConverter(typeof(EnumMemberConverter))]
        private enum EnumMemberConverterEnum
        {
            Hello,
            [EnumMember(Value = "world")]
            World,
        }

        [Theory]
        [InlineData(typeof(EnumMemberConverterEnum), new[] { "Hello", "world" })]
        public void IsEnum_True_WhenTypeIsEnumMemberConverterEnum(Type type, string[] members)
        {
            var options = new AsyncApiOptions();
            type.IsEnum(options, out var actualMembers).ShouldBeTrue();
            actualMembers.MemberType.ShouldBe(typeof(string));
            actualMembers.Members.ShouldBe(members);
        }

        private enum SomeEnum
        {
            Hello,
            World,
        }

        [Theory]
        [InlineData(typeof(SomeEnum), new[] { 0, 1 })]
        public void IsEnum_True_WhenTypeIsEnum(Type type, int[] members)
        {
            var options = new AsyncApiOptions();
            type.IsEnum(options, out var actualMembers).ShouldBeTrue();
            actualMembers.MemberType.ShouldBe(typeof(int));
            actualMembers.Members.ShouldBe(members);
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(string[]))]
        public void IsEnum_False_WhenTypeIsNotEnum(Type type)
        {
            var options = new AsyncApiOptions();
            type.IsEnum(options, out _).ShouldBeFalse();
        }

        [Theory]
        [InlineData(typeof(DateTime))]
        [InlineData(typeof(DateTime?))]
        [InlineData(typeof(DateTimeOffset))]
        [InlineData(typeof(DateTimeOffset?))]
        public void IsDateTime_True_WhenTypeIsDateTime(Type type)
        {
            type.IsDateTime().ShouldBeTrue();
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(string))]
        public void IsDateTime_False_WhenTypeIsDateTime(Type type)
        {
            type.IsDateTime().ShouldBeFalse();
        }

        [Theory]
        [InlineData(typeof(TimeSpan))]
        [InlineData(typeof(TimeSpan?))]
        public void IsDateTime_True_WhenTypeIsTimeSpan(Type type)
        {
            type.IsTimeSpan().ShouldBeTrue();
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(string))]
        public void IsDateTime_False_WhenTypeIsTimeSpan(Type type)
        {
            type.IsTimeSpan().ShouldBeFalse();
        }

        private class SomeClass
        {
            public string PropertyWithNothing { get; set; }

            [Display(Name = "title", Description = "description")]
            public string PropertyWithDisplayAttribute { get; set; }

            [Description("description")]
            public string PropertyWithDescriptionAttribute { get; set; }

            /// <summary>
            /// description
            /// </summary>
            /// <example>
            /// example
            /// </example>
            public string PropertyWithXmlComments { get; set; }

            [Range(1, 3.33)]
            public decimal PropertyWithRangeAttribute { get; set; }

            [MinLength(1)]
            [MaxLength(33)]
            public string[] ArrayPropertyWithMinMaxLengthAttributes { get; set; }

            [MinLength(1)]
            [MaxLength(33)]
            public string StringPropertyWithMinMaxLengthAttributes { get; set; }

            [StringLength(33, MinimumLength = 1)]
            public string PropertyWithStringLengthAttribute { get; set; }

            public ISet<string> ISetProperty { get; set; }

            public HashSet<string> HashSetProperty { get; set; }

            public IList<string> IListProperty { get; set; }

            [RegularExpression("pattern")]
            public string PropertyWithRegularExpressionAttribute { get; set; }

            [Required]
            public string PropertyWithRequiredAttribute { get; set; }

            //Fields

            public readonly string FieldWithNothing;

            [Display(Name = "title", Description = "description")]
            public readonly string FieldWithDisplayAttribute;

            [Description("description")]
            public string FieldWithDescriptionAttribute;

            /// <summary>
            /// description
            /// </summary>
            /// <example>
            /// example
            /// </example>
            public readonly string FieldWithXmlComments;

            [Range(1, 3.33)]
            public readonly decimal FieldWithRangeAttribute;

            [MinLength(1)]
            [MaxLength(33)]
            public readonly string[] ArrayFieldWithMinMaxLengthAttributes;

            [MinLength(1)]
            [MaxLength(33)]
            public readonly string StringFieldWithMinMaxLengthAttributes;

            [StringLength(33, MinimumLength = 1)]
            public readonly string FieldWithStringLengthAttribute;

            public readonly ISet<string> ISetField;

            public readonly HashSet<string> HashSetField;

            public readonly IList<string> IListField;

            [RegularExpression("pattern")]
            public readonly string FieldWithRegularExpressionAttribute;

            [Required]
            public readonly string FieldWithRequiredAttribute;

            public SomeClass(string fieldWithNothing, decimal fieldWithRangeAttribute,
                string[] arrayFieldWithMinMaxLengthAttributes, string stringFieldWithMinMaxLengthAttributes,
                string fieldWithStringLengthAttribute, ISet<string> setField, HashSet<string> hashSetField,
                IList<string> listField, string fieldWithRegularExpressionAttribute, string fieldWithRequiredAttribute,
                string fieldWithDisplayAttribute, string fieldWithDescriptionAttribute, string fieldWithXmlComments)
            {
                FieldWithNothing = fieldWithNothing;
                FieldWithRangeAttribute = fieldWithRangeAttribute;
                ArrayFieldWithMinMaxLengthAttributes = arrayFieldWithMinMaxLengthAttributes;
                StringFieldWithMinMaxLengthAttributes = stringFieldWithMinMaxLengthAttributes;
                FieldWithStringLengthAttribute = fieldWithStringLengthAttribute;
                ISetField = setField;
                HashSetField = hashSetField;
                IListField = listField;
                FieldWithRegularExpressionAttribute = fieldWithRegularExpressionAttribute;
                FieldWithRequiredAttribute = fieldWithRequiredAttribute;
                FieldWithDisplayAttribute = fieldWithDisplayAttribute;
                FieldWithDescriptionAttribute = fieldWithDescriptionAttribute;
                FieldWithXmlComments = fieldWithXmlComments;
            }
        }

        [Fact]
        public void GetTitle_ReturnsTitle_FromDisplayNameAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithDisplayAttribute));
            prop.GetTitle().ShouldBe("title");

            var field = typeof(SomeClass).GetField(nameof(SomeClass.FieldWithDisplayAttribute));
            field.GetTitle().ShouldBe("title");
        }

        [Fact]
        public void GetTitle_ReturnsNull_WhenNoTitle()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithNothing));
            prop.GetTitle().ShouldBeNull();

            var field = typeof(SomeClass).GetField(nameof(SomeClass.FieldWithNothing));
            field.GetTitle().ShouldBeNull();
        }

        [Fact]
        public void GetDescription_ReturnsDescription_FromDescriptionAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithDescriptionAttribute));
            prop.GetDescription().ShouldBe("description");

            var field = typeof(SomeClass).GetField(nameof(SomeClass.FieldWithDescriptionAttribute));
            field.GetDescription().ShouldBe("description");
        }

        [Fact]
        public void GetDescription_ReturnsDescription_FromDisplayAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithDisplayAttribute));
            prop.GetDescription().ShouldBe("description");

            var field = typeof(SomeClass).GetField(nameof(SomeClass.FieldWithDisplayAttribute));
            field.GetDescription().ShouldBe("description");
        }

        [Fact]
        public void GetDescription_ReturnsDescription_FromXmlSummaryTag()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithXmlComments));
            prop.GetDescription().ShouldBe("description");

            var field = typeof(SomeClass).GetField(nameof(SomeClass.FieldWithXmlComments));
            field.GetDescription().ShouldBe("description");
        }

        [Fact]
        public void GetMinimum_ReturnsMinimum_FromRangeAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithRangeAttribute));
            prop.GetMinimum().ShouldBe(1M);

            var field = typeof(SomeClass).GetField(nameof(SomeClass.FieldWithRangeAttribute));
            field.GetMinimum().ShouldBe(1M);
        }

        [Fact]
        public void GetMaximum_ReturnsMaximum_FromRangeAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithRangeAttribute));
            prop.GetMaximum().ShouldBe(3.33M);

            var field = typeof(SomeClass).GetField(nameof(SomeClass.FieldWithRangeAttribute));
            field.GetMaximum().ShouldBe(3.33M);
        }

        [Fact]
        public void GetMinItems_ReturnsMinItems_FromMinLengthAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.ArrayPropertyWithMinMaxLengthAttributes));
            prop.GetMinItems().ShouldBe(1);

            var field = typeof(SomeClass).GetField(nameof(SomeClass.ArrayFieldWithMinMaxLengthAttributes));
            field.GetMinItems().ShouldBe(1);
        }

        [Fact]
        public void GetMaxItems_ReturnsMaxItems_FromMaxLengthAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.ArrayPropertyWithMinMaxLengthAttributes));
            prop.GetMaxItems().ShouldBe(33);

            var field = typeof(SomeClass).GetField(nameof(SomeClass.ArrayFieldWithMinMaxLengthAttributes));
            field.GetMaxItems().ShouldBe(33);
        }

        [Fact]
        public void GetMinLength_ReturnsMinLength_FromMinLengthAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.StringPropertyWithMinMaxLengthAttributes));
            prop.GetMinLength().ShouldBe(1);

            var field = typeof(SomeClass).GetField(nameof(SomeClass.StringFieldWithMinMaxLengthAttributes));
            field.GetMinLength().ShouldBe(1);
        }

        [Fact]
        public void GetMinLength_ReturnsMinLength_FromStringLengthAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithStringLengthAttribute));
            prop.GetMinLength().ShouldBe(1);

            var field = typeof(SomeClass).GetField(nameof(SomeClass.FieldWithStringLengthAttribute));
            field.GetMinLength().ShouldBe(1);
        }

        [Fact]
        public void GetMaxLength_ReturnsMaxLength_FromMaxLengthAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.StringPropertyWithMinMaxLengthAttributes));
            prop.GetMaxLength().ShouldBe(33);

            var field = typeof(SomeClass).GetField(nameof(SomeClass.StringFieldWithMinMaxLengthAttributes));
            field.GetMaxLength().ShouldBe(33);
        }

        [Fact]
        public void GetMaxLength_ReturnsMaxLength_FromStringLengthAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithStringLengthAttribute));
            prop.GetMaxLength().ShouldBe(33);

            var field = typeof(SomeClass).GetField(nameof(SomeClass.FieldWithStringLengthAttribute));
            field.GetMaxLength().ShouldBe(33);
        }

        [Theory]
        [InlineData(nameof(SomeClass.ISetProperty))]
        [InlineData(nameof(SomeClass.HashSetProperty))]
        public void GetIsUniqueItemsWithProperty_True_WhenTypeIsSet(string propertyName)
        {
            var prop = typeof(SomeClass).GetProperty(propertyName);
            prop.GetIsUniqueItems().ShouldBe(true);
        }

        [Theory]
        [InlineData(nameof(SomeClass.ISetField))]
        [InlineData(nameof(SomeClass.HashSetField))]
        public void GetIsUniqueItemsWithField_True_WhenTypeIsSet(string fieldName)
        {
            var field = typeof(SomeClass).GetField(fieldName);
            field.GetIsUniqueItems().ShouldBe(true);
        }

        [Theory]
        [InlineData(nameof(SomeClass.IListProperty))]
        public void GetIsUniqueItemsWithProperty_False_WhenTypeIsNotSet(string propertyName)
        {
            var prop = typeof(SomeClass).GetProperty(propertyName);
            prop.GetIsUniqueItems().ShouldBe(false);
        }
        [Theory]
        [InlineData(nameof(SomeClass.IListField))]
        public void GetIsUniqueItemsWithField_False_WhenTypeIsNotSet(string fieldName)
        {
            var field = typeof(SomeClass).GetField(fieldName);
            field.GetIsUniqueItems().ShouldBe(false);
        }

        [Fact]
        public void GetPattern_ReturnsPattern_FromRegularExpressionAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithRegularExpressionAttribute));
            prop.GetPattern().ShouldBe("pattern");

            var field = typeof(SomeClass).GetField(nameof(SomeClass.FieldWithRegularExpressionAttribute));
            field.GetPattern().ShouldBe("pattern");
        }

        [Fact]
        public void GetIsRequired_ReturnsTrue_WhenPropertyOrFieldHasRequiredAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithRequiredAttribute));
            prop.GetIsRequired().ShouldBeTrue();

            var field = typeof(SomeClass).GetField(nameof(SomeClass.FieldWithRequiredAttribute));
            field.GetIsRequired().ShouldBeTrue();
        }

        [Fact]
        public void GetIsRequired_ReturnsTrue_WhenPropertyOrFieldDoesNotHaveRequiredAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithNothing));
            prop.GetIsRequired().ShouldBeFalse();

            var field = typeof(SomeClass).GetField(nameof(SomeClass.FieldWithNothing));
            field.GetIsRequired().ShouldBeFalse();
        }

        [Fact]
        public void GetExample_ReturnsExample_FromXmlCommentExampleTag()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithXmlComments));
            prop.GetExample().ShouldBe("example");

            var field = typeof(SomeClass).GetField(nameof(SomeClass.FieldWithXmlComments));
            field.GetExample().ShouldBe("example");
        }

        [Fact]
        public void GetExample_ReturnsNull_WhenNoXmlCommentExampleTag()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithNothing));
            prop.GetExample().ShouldBeNull();

            var field = typeof(SomeClass).GetField(nameof(SomeClass.FieldWithNothing));
            field.GetExample().ShouldBeNull();
        }
    }
}