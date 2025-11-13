using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static Presentation.ApiConstants.ApiConstants;

namespace Presentation.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An unhandled exception occurred while processing the request.");

            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = ContentType;

        var response = new
        {
            Message = "An unexpected error occurred. Please try again later."
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}
