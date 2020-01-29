using AssociadoFantastico.WebApi.Middleware;
using Microsoft.AspNetCore.Builder;

namespace AssociadoFantastico.WebApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHttpErrorMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpErrorMiddleware>();
        }

    }
}
