using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Linq;

namespace IncrGenerator;

internal class ComponentIncrementalGenerator
{
    public void Generate(string componentName, IncrementalGeneratorInitializationContext context)
    {
        string attributeName = $"AsyncApi{componentName}Attribute";

        string attributeSrc = @$"
using AsyncApiLibrary.Schema.v2;

namespace AsyncApi.Net.Generator
{{
    [AttributeUsage(AttributeTargets.Field)]
    public class {attributeName}: Attribute;
}}
";

        context.RegisterPostInitializationOutput(c => 
            c.AddSource($"{attributeName}.Generated.cs", 
            attributeSrc));

        IncrementalValuesProvider<FieldToGenerate?> filedToGenerates = context.SyntaxProvider
             .CreateSyntaxProvider(
                 predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                 transform: (c, _) => GetSemanticTargetForGeneration(c, attributeName))
             .Where(static m => m is not null);

        context.RegisterSourceOutput(filedToGenerates, (c, p) =>
        {
            if (!p.HasValue) { return; }

            VariableDeclaratorSyntax? variableDeclarator = p.Value.Field.Declaration.Variables.FirstOrDefault();
            if (variableDeclarator is null || variableDeclarator.Initializer is null) { return; }

            ExpressionSyntax body = variableDeclarator.Initializer.Value;
            TypeSyntax fieldType = p.Value.Field.Declaration.Type;

            string fieldName = variableDeclarator.Identifier.Text;

            if (fieldName.Length == 0)
            {
                return;
            }

            fieldName = fieldName.TrimStart('_');

            char startValue = char.ToUpperInvariant(fieldName[0]);

            fieldName = startValue + fieldName.Remove(0, 1);

            if (fieldName.Length == 0)
            {
                return;
            }

            string source = $@"
using AsyncApiLibrary.Schema.v2;
using Newtonsoft.Json.Schema;

namespace AsyncApi.Net.Generator
{{
    public partial class AsyncApiStore {{
        public partial class {componentName}s
        {{
            public static {fieldType} {fieldName} => {body.ToFullString()}; 
        }}
    }}
}}
";
            c.AddSource($"{componentName}.{fieldName}.Generated.cs", source);
        });
    }

    static bool IsSyntaxTargetForGeneration(SyntaxNode node)
    {
        return node is FieldDeclarationSyntax m && m.AttributeLists.Count > 0;
    }

    static FieldToGenerate? GetSemanticTargetForGeneration(GeneratorSyntaxContext context, string target)
    {
        FieldDeclarationSyntax declarationSyntax = (FieldDeclarationSyntax)context.Node;

        foreach (AttributeListSyntax attributeListSyntax in declarationSyntax.AttributeLists)
        {
            foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
            {
                TypeInfo typeInfo = context.SemanticModel.GetTypeInfo(attributeSyntax);

                if (typeInfo.Type?.Name == target)
                {
                    return new(declarationSyntax, attributeSyntax);
                }
            }
        }

        return null;
    }
}
