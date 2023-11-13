using System;
using System.Reflection;

using AsyncApi.Net.Generator.AsyncApiSchema.v2;

namespace AsyncApi.Net.Generator.Generation;

public interface IDocumentGenerator
{
    AsyncApiDocument GenerateDocument(TypeInfo[] assemblyTypes, AsyncApiOptions options, AsyncApiDocument prototype, IServiceProvider serviceProvider);
}