using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2
{
    public class License
    {
        public License(string name)
        {
            Name = name;
        }
        
        /// <summary>
        /// The license name used for the API.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; }

        /// <summary>
        /// A URL to the license used for the API.
        /// MUST be in the format of a URL.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}