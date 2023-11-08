using System;
using System.Reflection;

using Saunter.AsyncApiSchema.v2;

namespace Saunter.Generation;

public interface IDocumentGenerator
{
    AsyncApiDocument GenerateDocument(TypeInfo[] assemblyTypes, AsyncApiOptions options, AsyncApiDocument prototype, IServiceProvider serviceProvider);
}