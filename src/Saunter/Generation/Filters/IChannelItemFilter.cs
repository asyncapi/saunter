using System.Reflection;

using NJsonSchema.Generation;

using Saunter.AsyncApiSchema.v2;

namespace Saunter.Generation.Filters;

public interface IChannelItemFilter
{
    void Apply(ChannelItem channelItem, ChannelItemFilterContext context);
}

public class ChannelItemFilterContext
{
    public ChannelItemFilterContext(MemberInfo member, JsonSchemaResolver schemaResolver, JsonSchemaGenerator schemaGenerator)
    {
        Member = member;
        SchemaResolver = schemaResolver;
        SchemaGenerator = schemaGenerator;
    }

    public MemberInfo Member { get; }

    public JsonSchemaResolver SchemaResolver { get; }

    public JsonSchemaGenerator SchemaGenerator { get; }
}