using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.Serialization;
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

        private enum SomeEnum
        {
            [EnumMember(Value = "hello")] Hello,
            World,
        }

        [Theory]
        [InlineData(typeof(SomeEnum), new[] { "hello", "World" })]
        public void IsEnum_True_WhenTypeIsEnum(Type type, string[] members)
        {
            type.IsEnum(out var actualMembers).ShouldBeTrue();
            actualMembers.ShouldBe(members);
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(string[]))]
        public void IsEnum_False_WhenTypeIsNotEnum(Type type)
        {
            type.IsEnum(out _).ShouldBeFalse();
        }

        [Theory]
        [InlineData(typeof(DateTime))]
        [InlineData(typeof(DateTime?))]
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
        }

        [Fact]
        public void GetTitle_ReturnsTitle_FromDisplayNameAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithDisplayAttribute));
            prop.GetTitle().ShouldBe("title");
        }

        [Fact]
        public void GetTitle_ReturnsNull_WhenNoTitle()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithNothing));
            prop.GetTitle().ShouldBeNull();
        }

        [Fact]
        public void GetDescription_ReturnsDescription_FromDescriptionAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithDescriptionAttribute));
            prop.GetDescription().ShouldBe("description");
        }

        [Fact]
        public void GetDescription_ReturnsDescription_FromDisplayAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithDisplayAttribute));
            prop.GetDescription().ShouldBe("description");
        }

        [Fact]
        public void GetDescription_ReturnsDescription_FromXmlSummaryTag()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithXmlComments));
            prop.GetDescription().ShouldBe("description");
        }

        [Fact]
        public void GetMinimum_ReturnsMinimum_FromRangeAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithRangeAttribute));
            prop.GetMinimum().ShouldBe(1M);
        }
        
        [Fact]
        public void GetMaximum_ReturnsMaximum_FromRangeAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithRangeAttribute));
            prop.GetMaximum().ShouldBe(3.33M);
        }

        [Fact]
        public void GetMinItems_ReturnsMinItems_FromMinLengthAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.ArrayPropertyWithMinMaxLengthAttributes));
            prop.GetMinItems().ShouldBe(1);
        }
        
        [Fact]
        public void GetMaxItems_ReturnsMaxItems_FromMaxLengthAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.ArrayPropertyWithMinMaxLengthAttributes));
            prop.GetMaxItems().ShouldBe(33);
        }

        [Fact]
        public void GetMinLength_ReturnsMinLength_FromMinLengthAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.StringPropertyWithMinMaxLengthAttributes));
            prop.GetMinLength().ShouldBe(1);
        }

        [Fact]
        public void GetMinLength_ReturnsMinLength_FromStringLengthAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithStringLengthAttribute));
            prop.GetMinLength().ShouldBe(1);
        }
        
        [Fact]
        public void GetMaxLength_ReturnsMaxLength_FromMaxLengthAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.StringPropertyWithMinMaxLengthAttributes));
            prop.GetMaxLength().ShouldBe(33);
        }

        [Fact]
        public void GetMaxLength_ReturnsMaxLength_FromStringLengthAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithStringLengthAttribute));
            prop.GetMaxLength().ShouldBe(33);
        }

        [Theory]
        [InlineData(nameof(SomeClass.ISetProperty))]
        [InlineData(nameof(SomeClass.HashSetProperty))]
        public void GetIsUniqueItems_True_WhenTypeIsSet(string propertyName)
        {
            var prop = typeof(SomeClass).GetProperty(propertyName);
            prop.GetIsUniqueItems().ShouldBe(true);
        }
        
        [Theory]
        [InlineData(nameof(SomeClass.IListProperty))]
        public void GetIsUniqueItems_False_WhenTypeIsNotSet(string propertyName)
        {
            var prop = typeof(SomeClass).GetProperty(propertyName);
            prop.GetIsUniqueItems().ShouldBe(false);
        }

        [Fact]
        public void GetPattern_ReturnsPattern_FromRegularExpressionAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithRegularExpressionAttribute));
            prop.GetPattern().ShouldBe("pattern");
        }

        [Fact]
        public void GetIsRequired_ReturnsTrue_WhenPropertyHasRequiredAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithRequiredAttribute));
            prop.GetIsRequired().ShouldBeTrue();
        }
        
        [Fact]
        public void GetIsRequired_ReturnsTrue_WhenPropertyDoesNotHaveRequiredAttribute()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithNothing));
            prop.GetIsRequired().ShouldBeFalse();
        }

        [Fact]
        public void GetExample_ReturnsExample_FromXmlCommentExampleTag()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithXmlComments));
            prop.GetExample().ShouldBe("example");
        }
        
        [Fact]
        public void GetExample_ReturnsNull_WhenNoXmlCommentExampleTag()
        {
            var prop = typeof(SomeClass).GetProperty(nameof(SomeClass.PropertyWithNothing));
            prop.GetExample().ShouldBeNull();
        }
    }
}