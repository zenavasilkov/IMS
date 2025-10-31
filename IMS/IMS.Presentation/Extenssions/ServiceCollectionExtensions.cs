using IMS.BLL.Extensions;
using IMS.Presentation.HealthChecks;
using IMS.Presentation.Mapping;
using FluentValidation.AspNetCore;
using FluentValidation;

namespace IMS.Presentation.Extenssions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddBusinessLayerDedendencies(configuration)
            .AddMapping()
            .AddAllHealthChecks()
            .AddValidation();

        return services;
    }

    private static IServiceCollection AddMapping(this IServiceCollection services)
    {
        return services.AddAutoMapper(cfg => cfg.AddProfile<DtoMappingProfile>());
    }

    private static IServiceCollection AddAllHealthChecks(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("Database");

        return services;
    }

    private static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation()
        .AddFluentValidationClientsideAdapters()
        .AddValidatorsFromAssemblyContaining<Program>();

        return services;
    }
}
