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

        app.ApplyMigrations();
        
        app.UseHttpsRedirection();

        app.UseCors(AllowFrontend);

        app.UseAuthentication();
        app.UseAuthorization(); 
        
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.MapControllers();

        app.Run();
    }
}
