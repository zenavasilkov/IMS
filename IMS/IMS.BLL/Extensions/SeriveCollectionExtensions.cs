using IMS.BLL.Mapping;
using IMS.BLL.Services;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Extensions; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 

namespace IMS.BLL.Extensions
{
    public static class SeriveCollectionExtensions
    {
        public static IServiceCollection AddBusinessLayerDedendencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.
                AddDataLayerDependencies(configuration)
                .AddMapping()
                .AddServices();

            return services;
        }

        public static IServiceCollection AddMapping(this IServiceCollection services)
        {
            return services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>()
                    .AddScoped<ITicketService, TicketService>() 
                    .AddScoped<IBoardService, BoardService>();

            return services;
        }
    }
}
