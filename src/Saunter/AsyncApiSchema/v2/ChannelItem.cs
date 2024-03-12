using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Saunter.AsyncApiSchema.v2.Bindings;

namespace Saunter.AsyncApiSchema.v2
{
    /// <summary>
    /// Describes the operations available on a single channel.
    /// </summary>
    [JsonConverter(typeof(XParamsConverter))]
    public class ChannelItem
    {
        /// <summary>
        /// An optional description of this channel item.
        /// CommonMark syntax can be used for rich text representation.
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// A definition of the SUBSCRIBE operation.
        /// </summary>
        [JsonProperty("subscribe", NullValueHandling = NullValueHandling.Ignore)]
        public Operation Subscribe { get; set; }

        /// <summary>
        /// A definition of the PUBLISH operation.
        /// </summary>
        [JsonProperty("publish", NullValueHandling = NullValueHandling.Ignore)]
        public Operation Publish { get; set; }

        /// <summary>
        /// A map of the parameters included in the channel name.
        /// It SHOULD be present only when using channels with expressions
        /// (as defined by RFC 6570 section 2.2).
        /// </summary>
        [JsonProperty("parameters", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, IParameter> Parameters { get; set; } = new Dictionary<string, IParameter>();

        /// <summary>
        /// A free-form map where the keys describe the name of the protocol
        /// and the values describe protocol-specific definitions for the channel.
        /// </summary>
        [JsonProperty("bindings", NullValueHandling = NullValueHandling.Ignore)]
        public IChannelBindings Bindings { get; set; }

        /// <summary>
        /// The servers on which this channel is available, specified as an optional unordered
        /// list of names (string keys) of Server Objects defined in the Servers Object (a map).
        /// If servers is absent or empty then this channel must be available on all servers
        /// defined in the Servers Object.
        /// </summary>
        [JsonProperty("servers", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Servers { get; set; } = new List<string>();

        /// <summary>
        /// Specification Extensions. The extensions properties are implemented as patterned fields that are always prefixed by "x-" and must be format 'key=value'
        /// </summary>
        [JsonIgnore]
        public string[] XParams { get; set; }

        public bool ShouldSerializeParameters()
        {
            return Parameters != null && Parameters.Count > 0;
        }

        public bool ShouldSerializeServers()
        {
            return Servers != null && Servers.Count > 0;
        }
    }

    internal class XParamsConverter : JsonConverter
    {
        [ThreadStatic]
        static bool cannotWrite;

        // Disables the converter in a thread-safe manner.
        bool CannotWrite { get { return cannotWrite; } set { cannotWrite = value; } }

        public override bool CanWrite { get { return !CannotWrite; } }
        public struct PushValue<T> : IDisposable
        {
            Action<T> setValue;
            T oldValue;

            public PushValue(T value, Func<T> getValue, Action<T> setValue)
            {
                if (getValue == null || setValue == null)
                    throw new ArgumentNullException();
                this.setValue = setValue;
                this.oldValue = getValue();
                setValue(value);
            }

            #region IDisposable Members

            // By using a disposable struct we avoid the overhead of allocating and freeing an instance of a finalizable class.
            public void Dispose()
            {
                if (setValue != null)
                    setValue(oldValue);
            }

            #endregion
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            // Disabling writing prevents infinite recursion.
            using (new PushValue<bool>(true, () => CannotWrite, val => CannotWrite = val))
            {
                var obj = JObject.FromObject(value, serializer);

                var ci = value as ChannelItem;
                if (ci != null && ci.XParams != null && ci.XParams.Any())
                {
                    foreach (var xparam in ci.XParams)
                    {
                        if (!string.IsNullOrEmpty(xparam) && xparam.Count(x => x == '=') == 1)
                        {
                            var splitParam = xparam.Trim().Split('=');
                            if (splitParam.Length == 2)
                            {
                                var xParamNode = new JProperty($"x-{splitParam[0]}", new JValue(splitParam[1]));
                                obj.Add(xParamNode);
                            }
                            else
                            {
                                throw new FormatException($"XParams format is not correct. Use 'key=value'");
                            }
                        }
                        else
                        {
                            throw new FormatException($"XParams format is not correct. Use 'key=value'");
                        }
                    }
                }
                obj.WriteTo(writer);
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ChannelItem).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}