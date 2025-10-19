using System.Net;

namespace IMS.Presentation.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch(Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        logger.LogError(exception, "Unhandled exception occurred.");

        //TODO: Add custom exceptions and catch them

        var statusCode = HttpStatusCode.InternalServerError;

        var message = "An unexpected error occured.";

        var details = new ExceptionDetails(message, (int)statusCode, DateTime.UtcNow);

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = details.StatusCode;
        await httpContext.Response.WriteAsJsonAsync(details);
    }
}
