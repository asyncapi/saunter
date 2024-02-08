using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IncrGenerator;

internal readonly record struct FieldToGenerate
{
    public FieldToGenerate(FieldDeclarationSyntax field, AttributeSyntax attribute)
    {
        Field = field;
        Attribute = attribute;
    }

    public AttributeSyntax Attribute { get; }
    public FieldDeclarationSyntax Field { get; }
}
