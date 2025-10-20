using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IMS.DAL.Repositories.Interfaces;
using IMS.DAL.Repositories;
using IMS.DAL.Builders;

namespace IMS.DAL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataLayerDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext(configuration)
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        return services.AddDbContext<IMSDbContext>(options => options.UseNpgsql(connectionString));
    } 

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped(typeof(IRepository<>), typeof(Repository<>))
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ITicketRepository, TicketRepository>()
            .AddScoped<IInternshipRepository, InternshipRepository>()
            .AddScoped<IFeedbackRepository, FeedbackRepository>()
            .AddScoped<IBoardRepository, BoardRepository>()
            .AddScoped<ITicketFilterBuilder, TicketFilterBuilder>()
            .AddScoped<IFeedbackFilterBuilder, FeedbackFilterBuilder>()
            .AddScoped<IInternshipFilterBuilder, InternshipFilterBuilder>()
            .AddScoped<IUserFilterBuilder, UserFilterBuilder>();

        return services;
    }
}
