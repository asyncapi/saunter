using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace IncrGenerator.Tests;

[TestClass()]
public class IncrementalGeneratorTests
{
    [TestMethod()]
    public void InitializeTest()
    {
        string source = @"
using AsyncApi.Net.Generator;

public class Tester
{
    [AsyncApiOperation]
    private AsyncApiOperationDocument _myTester = new()
    {
        TestValue = ""sss""
    };
}
";

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);

        // Create a Roslyn compilation for the syntax tree.
        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: new[] { syntaxTree });

        var generator = new AsyncApiIncrementalGenerator();

        // The GeneratorDriver is used to run our generator against a compilation
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        // Run the source generator!
        driver = driver.RunGenerators(compilation);
    }
}