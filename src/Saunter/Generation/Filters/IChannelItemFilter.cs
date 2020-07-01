using System.Reflection;
using Saunter.AsyncApiSchema.v2;
using Saunter.Attributes;
using Saunter.Generation.SchemaGeneration;

namespace Saunter.Generation.Filters
{
    public interface IChannelItemFilter
    {
        void Apply(ChannelItem channelItem, ChannelItemFilterContext context);
    }

    public class ChannelItemFilterContext
    {
        public ChannelItemFilterContext(MemberInfo member, ISchemaRepository schemaRepository, ChannelAttribute channel)
        {
            Member = member;
            SchemaRepository = schemaRepository;
            Channel = channel;
        }
        
        public MemberInfo Member { get; }
        
        public ISchemaRepository SchemaRepository { get; }

        public ChannelAttribute Channel { get; }
    }
}