using IMS.BLL.Extensions;
using IMS.Presentation.HealthChecks;
using IMS.Presentation.Mapping;

namespace IMS.Presentation.Extenssions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddBusinessLayerDedendencies(configuration)
            .AddMapping()
            .AddAllHealthChecks();

        return services;
    }

    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        return services.AddAutoMapper(cfg => cfg.AddProfile<DtoMappingProfile>());
    }

    public static IServiceCollection AddAllHealthChecks(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("Database");

        return services;
    }
}
