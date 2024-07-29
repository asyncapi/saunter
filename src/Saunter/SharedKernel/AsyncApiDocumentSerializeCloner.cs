using LEGO.AsyncAPI;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers;
using Microsoft.Extensions.Logging;
using Saunter.SharedKernel.Interfaces;

namespace Saunter.SharedKernel
{
    internal class AsyncApiDocumentSerializeCloner : IAsyncApiDocumentCloner
    {
        private readonly ILogger<AsyncApiDocumentSerializeCloner> _logger;

        public AsyncApiDocumentSerializeCloner(ILogger<AsyncApiDocumentSerializeCloner> logger)
        {
            _logger = logger;
        }

        public AsyncApiDocument CloneProtype(AsyncApiDocument prototype)
        {
            var jsonView = prototype.Serialize(AsyncApiVersion.AsyncApi2_0, AsyncApiFormat.Json);

            var reader = new AsyncApiStringReader();

            var cloned = reader.Read(jsonView, out var diagnostic);

            if (diagnostic is not null)
            {
                foreach (var item in diagnostic.Errors)
                {
                    if (!item.Message.Contains("The field 'channels' in 'document' object is REQUIRED"))
                    {
                        _logger.LogError("Error while clone protype: {Error}", item);
                    }
                }

                foreach (var item in diagnostic.Warnings)
                {
                    _logger.LogWarning("Warning while clone protype: {Error}", item);
                }
            }

            cloned.Components ??= new();

            return cloned;
        }
    }
}
