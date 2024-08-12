using LEGO.AsyncAPI.Models;
using Saunter.Options.Filters;

public interface IOperationFilter
{
    void Apply(AsyncApiOperation operation, OperationFilterContext context);
}
