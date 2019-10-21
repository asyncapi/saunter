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
        public ChannelItemFilterContext(MethodInfo method, ISchemaRepository schemaRepository, ChannelAttribute channel)
        {
            Method = method;
            SchemaRepository = schemaRepository;
            Channel = channel;
        }
        
        public MethodInfo Method { get; }
        
        public ISchemaRepository SchemaRepository { get; }

        public ChannelAttribute Channel { get; }
    }
}