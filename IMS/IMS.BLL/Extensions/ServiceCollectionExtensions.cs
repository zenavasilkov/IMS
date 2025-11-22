using IMS.BLL.Mapping;
using IMS.BLL.Models;
using IMS.BLL.Services;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Extensions;
using IMS.NotificationsCore.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IMS.BLL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLayerDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDataLayerDependencies(configuration)
                .AddNotifications(configuration)
                .AddMapping()
                .AddServices()
                .AddAuth0Management(configuration)
                .AddMagicOnion();

            return services;
        }

        private static IServiceCollection AddMapping(this IServiceCollection services)
        {
            return services.AddAutoMapper(cfg => cfg.AddProfile<BllMappingProfile>());
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>()
                    .AddScoped<ITicketService, TicketService>() 
                    .AddScoped<IService<BoardModel, Board>, BoardService>()
                    .AddScoped<IInternshipService, InternshipService>()
                    .AddScoped<IFeedbackService, FeedbackService>();

            return services;
        }
        
        private static IServiceCollection AddAuth0Management(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IAuth0TokenProvider, Auth0TokenProvider>();
            services.AddSingleton<IAuth0ClientFactory, Auth0ClientFactory>();
            return services;
        }
    }
}
