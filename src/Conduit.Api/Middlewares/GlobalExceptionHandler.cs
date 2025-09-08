using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Middlewares;

internal sealed class GlobalExceptionHandler(
    RequestDelegate next,
    ILogger<GlobalExceptionHandler> logger
)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception occurred");

            httpContext.Response.StatusCode = exception switch
            {
                ApplicationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            await httpContext.Response.WriteAsJsonAsync(
                new ProblemDetails
                {
                    Type   = exception.GetType().Name,
                    Title  = "An error occurred",
                    Detail = exception.Message,
                }
            );
        }
    }
}
