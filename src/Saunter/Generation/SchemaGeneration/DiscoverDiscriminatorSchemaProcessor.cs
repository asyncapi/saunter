using System.Reflection;
using NJsonSchema.Generation;
using Saunter.Attributes;

namespace Saunter.Generation.SchemaGeneration
{
    public class DiscoverDiscriminatorSchemaProcessor : ISchemaProcessor
    {
        public void Process(SchemaProcessorContext context)
        {
            var attribute = context.Type.GetCustomAttribute<DiscriminatorAttribute>();
            if (attribute == null)
            {
                return;
            }

            var subtypes = context.Type.GetCustomAttributes<DiscriminatorSubTypeAttribute>();

            context.Schema.Discriminator = attribute.PropertyName;

            foreach (var subtype in subtypes)
            {
                //TODO: stackoverflow
                //context.Schema.OneOf.Add(context.Generator.Generate(subtype.SubType));
            }
        }
    }
}
