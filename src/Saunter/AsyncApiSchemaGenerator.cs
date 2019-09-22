using System;
using Microsoft.Extensions.Options;

namespace Saunter
{
    public class AsyncApiSchemaGenerator : IAsyncApiSchemaProvider
    {
        private readonly AsyncApiGeneratorOptions options;

        public AsyncApiSchemaGenerator(IOptions<AsyncApiGeneratorOptions> options)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }
        
        public AsyncApiSchema.v2_0_0.AsyncApiSchema GetSchema()
        {
            var asyncApiSchema = options.AsyncApiSchema;
            
            // todo: generate the channels section of the asyncapi schema
            
            // todo: validate asyncapi schema

            return asyncApiSchema;
        }
    }
}