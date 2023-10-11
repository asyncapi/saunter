using System;
using System.Reflection;

using Saunter.Utils;

using Shouldly;

using Xunit;

namespace Saunter.Tests.Utils;

public class ReflectionTests
{
    private class ExampleForTestingAttribute : Attribute { }

    [ExampleForTesting]
    private class TypeWithAttribute { }

    [Fact]
    public void HasCustomAttribute_True_WhenTypeHasCustomAttribute()
    {
        TypeInfo type = typeof(TypeWithAttribute).GetTypeInfo();

        type.HasCustomAttribute<ExampleForTestingAttribute>().ShouldBeTrue();
    }

    private class TypeWithoutAttribute { }

    [Fact]
    public void HasCustomAttribute_False_WhenTypeDoesNotHaveCustomAttribute()
    {
        TypeInfo type = typeof(TypeWithoutAttribute).GetTypeInfo();

        type.HasCustomAttribute<ExampleForTestingAttribute>().ShouldBeFalse();
    }
}