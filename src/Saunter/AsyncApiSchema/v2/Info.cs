using System;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2 {
    public class Info
    {
        public Info(string title, string version)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Version = version ?? throw new ArgumentNullException(nameof(version));
        }

        /// <summary>
        /// The title of the application.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; }

        /// <summary>
        /// Provides the version of the application API
        /// (not to be confused with the specification version).
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; }

        /// <summary>
        /// A short description of the application.
        /// CommonMark syntax can be used for rich text representation.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// A URL to the Terms of Service for the API
        /// MUST be in the format of a URL.
        /// </summary>
        [JsonProperty("termsOfService")]
        public string TermsOfService { get; set; }

        /// <summary>
        /// The contact information for the exposed API.
        /// </summary>
        [JsonProperty("contact")]
        public Contact Contact { get; set; }

        /// <summary>
        /// The license information for the exposed API.
        /// </summary>
        [JsonProperty("license")]
        public License License { get; set; }
    }
}