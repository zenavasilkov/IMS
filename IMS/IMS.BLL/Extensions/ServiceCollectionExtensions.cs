using IMS.BLL.BackgroundServices;
using IMS.BLL.Mapping;
using IMS.BLL.Models;
using IMS.BLL.Services;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Extensions;
using IMS.NotificationsCore.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace IMS.BLL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const int OutboxProcessingIntervalInMinutes = 1;
        
        public static IServiceCollection AddBusinessLayerDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDataLayerDependencies(configuration)
                .AddNotifications(configuration)
                .AddMapping()
                .AddServices()
                .AddAuth0Management()
                .AddOutboxProcessor()
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
        
        private static IServiceCollection AddAuth0Management(this IServiceCollection services)
        {
            services.AddSingleton<IAuth0TokenProvider, Auth0TokenProvider>();
            services.AddSingleton<IAuth0ClientFactory, Auth0ClientFactory>();
            return services;
        }
        
        private static IServiceCollection AddOutboxProcessor(this IServiceCollection services)
        {
            services.AddQuartz(configure =>
            {
                var jobKey = new JobKey(nameof(Auth0OutboxProcessor));

                configure
                    .AddJob<Auth0OutboxProcessor>(opts => opts.WithIdentity(jobKey))
                    .AddTrigger(trigger => trigger.ForJob(jobKey)
                        .WithSimpleSchedule(schedule => schedule
                            .WithIntervalInMinutes(OutboxProcessingIntervalInMinutes)
                            .RepeatForever()));
            });

            services.AddQuartzHostedService();

            return services;
        }
    }
}
