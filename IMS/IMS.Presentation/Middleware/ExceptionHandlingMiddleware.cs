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
            await HandleExceptionAsync(httpContext, ex, (int)HttpStatusCode.InternalServerError);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception, int statusCode)
    {
        httpContext.Response.StatusCode = statusCode;

        //TODO: Add custom exceptions and catch them

        var message = exception.Message;

        var details = new ExceptionDetails(message, statusCode, DateTime.UtcNow);

        httpContext.Response.ContentType = ApiConstants.ApiConstants.ContentType;

        logger.LogError("{message} {newLine} {innerExceptionMessage}", 
            exception.Message, Environment.NewLine, exception.InnerException?.Message);

        logger.LogError("Error query: {query}", httpContext.Request.Path);

        logger.LogError("{stackTrace}", exception.StackTrace);

        await httpContext.Response.WriteAsJsonAsync(details);
    }
}
