using IMS.BLL.Mapping;
using IMS.DAL.Extensions; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 

namespace IMS.BLL.Extensions
{
    public static class SeriveCollectionExtensions
    {
        public static IServiceCollection AddBusinessLayerDedendencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.
                AddDataLayerDependencies(configuration)
                .AddMapping();

            return services;
        }

        public static IServiceCollection AddMapping(this IServiceCollection services)
        {
            return services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
        }
    }
}
