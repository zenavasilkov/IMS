using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IMS.DAL.Repositories.Interfaces;
using IMS.DAL.Repositories;
using IMS.DAL.Builders;
using IMS.DAL.Interceptors;
using IMS.DAL.Caching;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace IMS.DAL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataLayerDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddRepositories()
            .AddRedis(configuration)
            .AddInterceptors()
            .AddDbContext(configuration);

        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ImsDefaultConnection");

        services.AddDbContext<ImsDbContext>((provider, options) =>
        {
            options.UseNpgsql(connectionString);

            var interceptors = provider.GetServices<IInterceptor>();
            options.AddInterceptors(interceptors);
        });
        
        return services;
    } 

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped(typeof(Repository<>))
            .AddScoped(typeof(IRepository<>), typeof(CachedRepository<>))
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ITicketRepository, TicketRepository>()
            .AddScoped<IInternshipRepository, InternshipRepository>()
            .AddScoped<IFeedbackRepository, FeedbackRepository>()
            .AddScoped<IBoardRepository, BoardRepository>()
            .AddScoped<ITicketFilterBuilder, TicketFilterBuilder>()
            .AddScoped<IFeedbackFilterBuilder, FeedbackFilterBuilder>()
            .AddScoped<IInternshipFilterBuilder, InternshipFilterBuilder>()
            .AddScoped<IUserFilterBuilder, UserFilterBuilder>()
            .AddScoped<UpdateTimestampsInterceptor>();

        return services;
    }

    private static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(redisOptions => 
            redisOptions.Configuration = configuration.GetConnectionString("Redis"));

        return services;
    }

    private static IServiceCollection AddInterceptors(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<CreateUserInterceptor>()
            .AddClasses(classes => classes.AssignableTo<IInterceptor>())
            .As<IInterceptor>()
            .WithSingletonLifetime());
        
        return services;
    }
}
