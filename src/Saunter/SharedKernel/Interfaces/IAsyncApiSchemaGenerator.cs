using System;
using LEGO.AsyncAPI.Models;

namespace Saunter.SharedKernel.Interfaces
{
    public interface IAsyncApiSchemaGenerator
    {
        AsyncApiSchema? Generate(Type? type);
    }
}
