using IMS.DAL.Extensions; 

namespace IMS.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //TODO: Change it to the appropriet state
            builder.Services.AddDataLayerDependencies(builder.Configuration);

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
