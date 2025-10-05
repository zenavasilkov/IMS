using IMS.DAL;
using Microsoft.EntityFrameworkCore;

namespace IMS.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<IMSDbContext>(options => options.
               UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers(); 
            builder.Services.AddOpenApi();

            var app = builder.Build(); 
             
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
}
