using Infrastructure;
using Serilog;

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
        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.ApplyMigrations();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
         
        app.UseHttpsRedirection(); 
        app.UseAuthorization(); 
        app.MapControllers();

        app.Run();
    }
}
