using System.Reflection;

using NJsonSchema.Generation;

using AsyncApi.Net.Generator.AsyncApiSchema.v2;

namespace AsyncApi.Net.Generator.Generation.Filters;

public interface IChannelItemFilter
{
    void Apply(Channel channelItem, ChannelItemFilterContext context);
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