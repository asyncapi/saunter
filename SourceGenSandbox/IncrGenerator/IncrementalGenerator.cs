using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Linq;

namespace IncrGenerator;

public readonly record struct FieldToGenerate
{
    public FieldToGenerate(FieldDeclarationSyntax field, AttributeSyntax attribute)
    {
        Field = field;
        Attribute = attribute;
    }

    public AttributeSyntax Attribute { get; }
    public FieldDeclarationSyntax Field { get; }
}

public static class Attributes
{
    public const string AsyncApiOperationAttribute = @$"
namespace AsyncApi.Net.Generator
{{
    [AttributeUsage(AttributeTargets.Field)]
    public class {nameof(AsyncApiOperationAttribute)} : Attribute;
}}
";

    public const string AsyncApiDocument = $@"
#nullable enable

namespace AsyncApi.Net.Generator
{{
    public class AsyncApiOperationDocument
    {{
        public string? TestValue {{ get; set; }}
    }}

    public static class {nameof(AsyncApiDocument)}
    {{
        public static AsyncApiOperations Operations => new();
    }}

    public partial class AsyncApiOperations;
}}
";
}

[Generator]
public class IncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(c => 
            c.AddSource($"{nameof(Attributes.AsyncApiOperationAttribute)}.Generated.cs", 
            Attributes.AsyncApiOperationAttribute));

        context.RegisterPostInitializationOutput(c => 
            c.AddSource($"{nameof(Attributes.AsyncApiDocument)}.Generated.cs", 
            Attributes.AsyncApiDocument));

        IncrementalValuesProvider<FieldToGenerate?> filedToGenerates = context.SyntaxProvider
             .CreateSyntaxProvider(
                 predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                 transform: static (c, _) => GetSemanticTargetForGeneration(c, nameof(Attributes.AsyncApiOperationAttribute)))
             .Where(static m => m is not null);

        context.RegisterSourceOutput(filedToGenerates, static (c, p) =>
        {
            if (!p.HasValue) { return; }

            VariableDeclaratorSyntax? variableDeclarator = p.Value.Field.Declaration.Variables.FirstOrDefault();
            if (variableDeclarator is null || variableDeclarator.Initializer is null) { return; }

            ExpressionSyntax body = variableDeclarator.Initializer.Value;
            TypeSyntax fieldType = p.Value.Field.Declaration.Type;

            string typeName = $"{p.Value.Attribute.Name}s";
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
namespace AsyncApi.Net.Generator
{{
    public partial class {typeName}
    {{
        public {fieldType} {fieldName} => {body}; 
    }}
}}
";
            c.AddSource($"{typeName}.{fieldName}.Generated.cs", source);
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
