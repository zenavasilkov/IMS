using IMS.BLL.Extensions;
using Microsoft.AspNetCore.Builder;

namespace IMS.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
             
            builder.Services.AddBusinessLayerDedendencies(builder.Configuration);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApi();

            builder.Services.AddSwaggerGen();

            var app = builder.Build(); 
             
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.RoutePrefix = string.Empty;
                }); 
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
             
            app.MapControllers();

            app.Run();
        }
    }
}
