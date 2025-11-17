using IMS.BLL.Extensions;
using IMS.DAL;
using IMS.Presentation.HealthChecks;
using IMS.Presentation.Mapping;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace IMS.Presentation.Extensions;

public static class Extensions
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddBusinessLayerDedendencies(configuration)
            .AddMapping()
            .AddAllHealthChecks()
            .AddValidation()
            .AddGrpc();

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

    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<ImsDbContext>();

        if (dbContext.Database.IsRelational())
        {
            dbContext.Database.Migrate();
        }
    }
}
