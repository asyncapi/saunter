using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging.Testing;
using Saunter.AttributeProvider;
using Saunter.Options;
using Saunter.SharedKernel;

namespace Saunter.Tests.Generation.DocumentGeneratorTests
{
    internal static class ArrangeAttributesTests
    {
        private sealed class FakeAsyncApiOptions : AsyncApiOptions
        {
            private readonly TypeInfo[] _types;

            public FakeAsyncApiOptions(Type[] types)
            {
                _types = types.Select(t => t.GetTypeInfo()).ToArray();
            }

            internal override IReadOnlyCollection<TypeInfo> AsyncApiSchemaTypes => _types;
        }

        public static void Arrange(out AsyncApiOptions options, out AttributeDocumentProvider documentProvider, params Type[] targetTypes)
        {
            options = new FakeAsyncApiOptions(targetTypes);
            documentProvider = new AttributeDocumentProvider(
                ActivatorServiceProvider.Instance,
                new AsyncApiSchemaGenerator(),
                new AsyncApiDocumentSerializeCloner(new FakeLogger<AsyncApiDocumentSerializeCloner>()));
        }
    }
}
