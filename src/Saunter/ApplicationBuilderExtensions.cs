using Microsoft.AspNetCore.Builder;
using Saunter.UI;

namespace Saunter;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseAsyncApi(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseMiddleware<AsyncApiMiddleware>();
        applicationBuilder.UseMiddleware<AsyncApiUiMiddleware>();

        return applicationBuilder;
    }
}