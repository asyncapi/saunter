using System;
using Microsoft.Extensions.Options;
using Saunter.Generation;
using Saunter.Generation.SchemaGeneration;

namespace Saunter.Tests
{
    public static class TestProviderFactory
    {
        public static IAsyncApiDocumentProvider Provider(Action<AsyncApiOptions> setupAction = null)
        {
            var options = new AsyncApiOptions();
            setupAction?.Invoke(options);
            var wrappedOptions = Options.Create(options);
            
            var schemaGenerator = new SchemaGenerator(wrappedOptions);
            var documentGenerator = new DocumentGenerator(Options.Create(options), schemaGenerator);
            
            return new AsyncApiDocumentProvider(Options.Create(options), documentGenerator);
        }
    }
}