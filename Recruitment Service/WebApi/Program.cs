using Infrastructure;
using Serilog;
using WebApi.Middleware;
using static Presentation.ApiConstants.ApiConstants;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog();

        builder.Services.AddDependencies(builder.Configuration);
        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseCors(AllowFrontend);

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.ApplyMigrations();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
         
        app.UseHttpsRedirection(); 
        app.UseAuthorization(); 
        app.MapControllers();

        app.Run();
    }
}
