using System;
using Microsoft.Extensions.Options;
using Saunter.Generation;
using Saunter.Generation.SchemaGeneration;

namespace Saunter.Tests
{
    public static class TestProviderFactory
    {
        public static IAsyncApiDocumentProvider Provider(Action<AsyncApiDocumentGeneratorOptions> setupAction = null)
        {
            var options = new AsyncApiDocumentGeneratorOptions();
            setupAction?.Invoke(options);
            var wrappedOptions = Options.Create(options);
            
            var schemaGenerator = new SchemaGenerator(wrappedOptions);
            
            return new AsyncApiDocumentGenerator(Options.Create(options), schemaGenerator);
        }
    }
}