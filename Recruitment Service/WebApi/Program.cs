using Infrastructure;
using Serilog;
using WebApi.Middleware;

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
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

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
