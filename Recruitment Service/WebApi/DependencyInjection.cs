using Application;
using Infrastructure;
using Presentation;

namespace WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services
            .AddPresentation()
            .AddInfrastructure()
            .AddApplication();

        return services;
    }
}
