using Saunter.AsyncApiSchema.v2;
using System.Text.Json;

namespace Saunter.Utils
{
    public class JsonAsyncApiDocumentSerializer
    {
        public string Serialize(AsyncApiDocument document)
        {
            return JsonSerializer.Serialize(
                document,
                new JsonSerializerOptions
                {
                    WriteIndented = false,
                    IgnoreNullValues = true,
                    Converters =
                    {
                        new DictionaryKeyToStringConverter(),
                        new InterfaceImplementationConverter(),
                    },
                }
            );
        }
    }
}