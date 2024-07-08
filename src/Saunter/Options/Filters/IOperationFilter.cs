using LEGO.AsyncAPI.Models;

public interface IOperationFilter
{
    void Apply(AsyncApiOperation operation, OperationFilterContext context);
}
