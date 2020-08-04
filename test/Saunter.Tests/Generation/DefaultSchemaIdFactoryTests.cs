using System.Collections.Generic;
using Newtonsoft.Json;
using Saunter.Generation;
using Shouldly;
using Xunit;

namespace Saunter.Tests.Generation
{
    public class DefaultSchemaIdFactoryTests
    {
        [Fact]
        public void Generate_UsesTitleFromJsonObject()
        {
            DefaultSchemaIdFactory.Generate(typeof(TypeWithJsonObjectAttribute))
                .ShouldBe("jsonObjectTitle");
        }

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

        [Fact]
        public void Generate_RemovesInvalidCharacters()
        {
            DefaultSchemaIdFactory.Generate(typeof(TypeWithInvalidCharacters))
                .ShouldBe("thisisinvalid");
        }
    }
    
    [JsonObject(Title = "jsonObjectTitle")]
    public class TypeWithJsonObjectAttribute {}

    public class OuterType
    {
        public class InnerType {}
    }
    
    public class Foo {}
    
    [JsonObject(Title = "This is invalid!")]
    public class TypeWithInvalidCharacters {}
}