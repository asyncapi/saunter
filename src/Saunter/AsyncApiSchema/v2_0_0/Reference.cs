using System;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2_0_0 {
    public class Reference
    {
        public Reference(string @ref)
        {
            Ref = @ref ?? throw new ArgumentNullException(nameof(@ref));
        }
        
        [JsonProperty("$ref")]
        public string Ref { get; }
    }
}