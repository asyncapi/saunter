using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Models;

namespace Saunter.SharedKernel.Interfaces
{
    public interface IAsyncApiSchemaGenerator
    {
        GeneratedSchemas? Generate(Type? type);
    }

    public readonly record struct GeneratedSchemas(AsyncApiSchema Root, IReadOnlyCollection<AsyncApiSchema> All);
}
