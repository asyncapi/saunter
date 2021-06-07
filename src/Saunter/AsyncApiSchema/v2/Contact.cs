using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2
{
    public class Contact
    {
        /// <summary>
        /// The identifying name of the contact person/organization.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The URL pointing to the contact information.
        /// MUST be in the format of a URL.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// The email address of the contact person/organization.
        /// MUST be in the format of an email address.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}