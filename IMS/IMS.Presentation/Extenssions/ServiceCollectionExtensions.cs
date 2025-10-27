using IMS.BLL.Extensions;
using IMS.Presentation.Mapping;

namespace IMS.Presentation.Extenssions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddBusinessLayerDedendencies(configuration)
            .AddMapping()
            .AddAllHealthChecks(configuration);

        return services;
    }

    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        return services.AddAutoMapper(cfg => cfg.AddProfile<DtoMappingProfile>());
    }

    public static IServiceCollection AddAllHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHealthChecks()
            .AddNpgSql(
                connectionString: configuration.GetConnectionString("DefaultConnection") ?? "",
                name: "PostgreSQL (Raw)",
                timeout: TimeSpan.FromSeconds(5),
                tags: ["db", "postgres"]);

        return services;
    }
}
