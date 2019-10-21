using System.Collections.Generic;

namespace Saunter.AsyncApiSchema.v2 
{
    public class Channels : Dictionary<ChannelsFieldName, ChannelItem>
    {
        public void AddRange(Channels channels)
        {
            if (channels == null)
            {
                return;
            }
            
            foreach (var channel in channels)
            {
                Add(channel.Key, channel.Value);
            }
        }
    }
}