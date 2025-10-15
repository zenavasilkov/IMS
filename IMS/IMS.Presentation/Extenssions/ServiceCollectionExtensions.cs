using IMS.BLL.Extensions;
using IMS.Presentation.Mapping;

namespace IMS.Presentation.Extenssions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBusinessLayerDedendencies(configuration)
            .AddMapping();

        return services;
    }

    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        return services.AddAutoMapper(cfg => cfg.AddProfile<DtoMappingProfile>());
    }
}
