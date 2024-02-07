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
    public const string AsyncApiChannelAttribute = @$"
using AsyncApiLibrary.Schema.v2;

namespace AsyncApi.Net.Generator
{{
    [AttributeUsage(AttributeTargets.Field)]
    public class {nameof(AsyncApiChannelAttribute)} : Attribute;
}}
";

    public const string AsyncApiDocument = $@"
#nullable enable

using AsyncApiLibrary.Schema.v2;

namespace AsyncApi.Net.Generator
{{
    public class {nameof(AsyncApiDocument)} : IAsyncApiDocument
    {{
        public {nameof(AsyncApiDocument)}(Info info){{
            Info = info;
        }}

        /// <summary>
        /// Specifies the AsyncAPI Specification version being used.
        /// </summary>
        public string? AsyncApi {{ get; }} = ""2.6.0"";

        /// <summary>
        /// Identifier of the application the AsyncAPI document is defining.
        /// </summary>
        public string? Id {{ get; set; }}

        /// <summary>
        /// Provides metadata about the API. The metadata can be used by the clients if needed.
        /// </summary>
        public Info Info {{ get; set; }}

        /// <summary>
        /// Provides connection details of servers.
        /// </summary>
        public Dictionary<string, Server> Servers {{ get; set; }} = [];

        /// <summary>
        /// A string representing the default content type to use when encoding/decoding a message's payload.
        /// The value MUST be a specific media type (e.g. application/json).
        /// </summary>
        public string? DefaultContentType {{ get; set; }} = ""application/json"";

        /// <summary>
        /// The available channels and messages for the API.
        /// </summary>
        public AsyncApiChannels Channels {{ get; set; }} = [];

        IDictionary<string, ChannelItem> IAsyncApiDocument.Channels {{ get => this.Channels; }}

        /// <summary>
        /// An element to hold various schemas for the specification.
        /// </summary>
        public Components Components {{ get; set; }} = new();

        /// <summary>
        /// A list of tags used by the specification with additional metadata.
        /// Each tag name in the list MUST be unique.
        /// </summary>
        public HashSet<Tag> Tags {{ get; set; }} = [];

        /// <summary>
        /// Additional external documentation.
        /// </summary>
        public ExternalDocumentation? ExternalDocs {{ get; set; }}
    }}

    public partial class AsyncApiChannels : Dictionary<string, ChannelItem>;
}}
";
}

[Generator]
public class IncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(c => 
            c.AddSource($"{nameof(Attributes.AsyncApiChannelAttribute)}.Generated.cs", 
            Attributes.AsyncApiChannelAttribute));

        context.RegisterPostInitializationOutput(c => 
            c.AddSource($"{nameof(Attributes.AsyncApiDocument)}.Generated.cs", 
            Attributes.AsyncApiDocument));

        IncrementalValuesProvider<FieldToGenerate?> filedToGenerates = context.SyntaxProvider
             .CreateSyntaxProvider(
                 predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                 transform: static (c, _) => GetSemanticTargetForGeneration(c, nameof(Attributes.AsyncApiChannelAttribute)))
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
using AsyncApiLibrary.Schema.v2;

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
