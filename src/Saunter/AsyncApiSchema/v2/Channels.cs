using System;
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
                AddOrAppend(channel.Key, channel.Value);
            }
        }

        public void AddOrAppend(ChannelsFieldName key, ChannelItem channel)
        {
            if (ContainsKey(key))
            {
                var existingItem = this[key];
                if (existingItem.Publish == null && channel.Publish != null)
                    this[key].Publish = channel.Publish;
                else if (existingItem.Publish != null && channel.Publish != null)
                    throw new ArgumentException($"An item with the same key and with an existing publish operation has already been added to the channel collection.");

                if (existingItem.Subscribe == null && channel.Subscribe != null)
                    this[key].Subscribe = channel.Subscribe;
                else if (existingItem.Subscribe != null && channel.Subscribe != null)
                    throw new ArgumentException($"An item with the same key and with an existing subscribe operation has already been added to the channel collection.");
            }
            else
                this[key] = channel;
        }
    }
}