using System.Reflection;

using NJsonSchema.Generation;

using Saunter.AsyncApiSchema.v2;
using Saunter.Attributes;

namespace Saunter.Generation.Filters;

public interface IChannelItemFilter
{
    void Apply(ChannelItem channelItem, ChannelItemFilterContext context);
}

public class ChannelItemFilterContext
{
    public ChannelItemFilterContext(MemberInfo member, JsonSchemaResolver schemaResolver, JsonSchemaGenerator schemaGenerator, ChannelAttribute channel)
    {
        Member = member;
        SchemaResolver = schemaResolver;
        SchemaGenerator = schemaGenerator;
        Channel = channel;
    }

    public MemberInfo Member { get; }

    public JsonSchemaResolver SchemaResolver { get; }

    public JsonSchemaGenerator SchemaGenerator { get; }

    public ChannelAttribute Channel { get; }
}
