using IMS.Presentation.Extenssions;
using IMS.Presentation.Middleware;
using Microsoft.AspNetCore.Builder;

namespace IMS.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
             
            builder.Services.AddApiDependencies(builder.Configuration); 

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(); 
            }

            app.UseHttpsRedirection();

            app.MapHealthChecks("/_health");

            app.UseAuthorization();
             
            app.MapControllers();

            app.Run();
        }
    }
}
