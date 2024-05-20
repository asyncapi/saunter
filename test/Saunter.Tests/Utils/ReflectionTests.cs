using System;
using System.Reflection;

using Saunter.Utils;

using Shouldly;

using Xunit;

namespace Saunter.Tests.Utils;

public class ReflectionTests
{
    [AttributeUsage(AttributeTargets.Class)]
    private class ExampleForTestingAttribute : Attribute { }

    [ExampleForTesting]
    private class TypeWithAttribute { }

    [Fact]
    public void HasCustomAttributeTrueWhenTypeHasCustomAttribute()
    {
        TypeInfo type = typeof(TypeWithAttribute).GetTypeInfo();

        type.HasCustomAttribute<ExampleForTestingAttribute>().ShouldBeTrue();
    }

    private class TypeWithoutAttribute { }

    [Fact]
    public void HasCustomAttributeFalseWhenTypeDoesNotHaveCustomAttribute()
    {
        TypeInfo type = typeof(TypeWithoutAttribute).GetTypeInfo();

        type.HasCustomAttribute<ExampleForTestingAttribute>().ShouldBeFalse();
    }
}
