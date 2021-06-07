using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Saunter.AsyncApiSchema.v2;

namespace Saunter.Utils
{
    public class NewtonsoftAsyncApiDocumentSerializer : IAsyncApiDocumentSerializer
    {
        public string ContentType => "application/json";
        public string Serialize(AsyncApiDocument document)
        {
            return JsonConvert.SerializeObject(document);
        }
    }
}
