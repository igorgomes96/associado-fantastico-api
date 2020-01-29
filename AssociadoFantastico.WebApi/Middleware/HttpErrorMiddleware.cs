using AssociadoFantastico.Application.Exceptions;
using AssociadoFantastico.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AssociadoFantastico.WebApi.Middleware
{
    public class HttpErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public HttpErrorMiddleware(RequestDelegate next, ILogger<HttpErrorMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (NotFoundException ex)
            {
                httpContext.Response.StatusCode = 404;
                await httpContext.Response.WriteAsync(ex.Message);
            }
            catch (DuplicatedException ex)
            {
                httpContext.Response.StatusCode = 409;
                await httpContext.Response.WriteAsync(ex.Message);
            }
            catch (UnauthorizedException ex)
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync(ex.Message);
            }
            catch (CustomException ex)
            {
                httpContext.Response.StatusCode = 400;
                _logger.LogWarning(ex, "Erro de negócio.");
                await httpContext.Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                httpContext.Response.StatusCode = 500;
                _logger.LogError(ex, "Exceção não tratada.");
                await httpContext.Response.WriteAsync("Ocorreu um erro inesperado!");
            }
        }
    }

}
