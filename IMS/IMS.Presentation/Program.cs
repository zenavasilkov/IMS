using HealthChecks.UI.Client;
using IMS.Presentation.Extensions;
using IMS.Presentation.Middleware;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

namespace IMS.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.ConfigureKestrel(ops =>
        {
            ops.ListenAnyIP(8080, o => o.Protocols = HttpProtocols.Http1);
            ops.ListenAnyIP(8081, o => o.Protocols = HttpProtocols.Http2);
        });
            
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog();
         
        builder.Services.AddApiDependencies(builder.Configuration); 

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
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

        app.MapHealthChecks("/_health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseAuthorization();
        
        app.MapMagicOnionService();

        app.MapControllers();

        app.Run();
    }
}
