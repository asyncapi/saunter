using System.Collections.Generic;
using Saunter.Generation;
using Shouldly;
using Xunit;

namespace Saunter.Tests.Generation
{
    public class DefaultSchemaIdFactoryTests
    {
        [Fact]
        public void Generate_NestedType_ConcatenatesTypeNames()
        {
            DefaultSchemaIdFactory.Generate(typeof(OuterType.InnerType))
                .ShouldBe("outerTypeInnerType");
        }

        [Fact]
        public void Generate_Generic1()
        {
            DefaultSchemaIdFactory.Generate(typeof(List<Foo>))
                .ShouldBe("listOfFoo");
        }
        
        [Fact]
        public void Generate_Generic2()
        {
            DefaultSchemaIdFactory.Generate(typeof(Dictionary<string, Foo>))
                .ShouldBe("dictionaryOfStringAndFoo");
        }
    }

    public class OuterType
    {
        public class InnerType {}
    }
    
    public class Foo {}
}