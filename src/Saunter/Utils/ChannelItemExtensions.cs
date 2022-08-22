using System;
using System.Collections.Generic;
using Saunter.AsyncApiSchema.v2;

namespace Saunter.Utils
{
    public static class ChannelItemExtensions
    {
        public static void AddRange(this IDictionary<string, ChannelItem> source, IDictionary<string, ChannelItem> channels)
        {
            if (channels == null)
            {
                return;
            }

            foreach (var channel in channels)
            {
                source.AddOrAppend(channel.Key, channel.Value);
            }
        }

        public static void AddOrAppend(this IDictionary<string, ChannelItem> source, string key, ChannelItem channel)
        {
            if (source.TryGetValue(key, out var existing))
            {
                if (existing.Publish != null && channel.Publish != null)
                    throw new ArgumentException("An item with the same key and with an existing publish operation has already been added to the channel collection.");
                
                if (existing.Subscribe != null && channel.Subscribe != null)
                    throw new ArgumentException("An item with the same key and with an existing subscribe operation has already been added to the channel collection.");

                existing.Description ??= channel.Description;
                existing.Parameters ??= channel.Parameters;
                existing.Publish ??= channel.Publish;
                existing.Subscribe ??= channel.Subscribe;
                existing.Bindings ??= channel.Bindings;
                existing.Servers ??= channel.Servers;
            }
            else
            {
                source.Add(key, channel);
            }
        }
    }
}
