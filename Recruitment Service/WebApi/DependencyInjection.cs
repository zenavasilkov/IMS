using Application;
using Infrastructure;
using Presentation;

namespace WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddPresentation(configuration)
            .AddInfrastructure(configuration)
            .AddApplication(configuration);

        return services;
    }
}
