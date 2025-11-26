using IMS.BLL.Exceptions;
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var message = exception.Message;

        var details = exception switch
        {
            NotFoundException => new ExceptionDetails(message, (int)HttpStatusCode.NotFound, DateTime.UtcNow),

            IncorrectAssignmentException => new ExceptionDetails(message, (int)HttpStatusCode.BadRequest, DateTime.UtcNow),
            
            EmailIsNotUniqueException => new ExceptionDetails(message, (int)HttpStatusCode.BadRequest, DateTime.UtcNow),

            _ => new ExceptionDetails(message, (int)HttpStatusCode.InternalServerError, DateTime.UtcNow)
        };

        httpContext.Response.ContentType = ApiConstants.ApiConstants.ContentType;
        httpContext.Response.StatusCode = details.StatusCode;

        logger.LogError("{message} {newLine} {innerExceptionMessage}", 
            exception.Message, Environment.NewLine, exception.InnerException?.Message);

        logger.LogError("Error query: {query}", httpContext.Request.Path);

        logger.LogError("{stackTrace}", exception.StackTrace);

        await httpContext.Response.WriteAsJsonAsync(details);
    }
}
