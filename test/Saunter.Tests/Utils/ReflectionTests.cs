using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
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
    }
}
