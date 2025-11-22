using IMS.BLL.Extensions;
using IMS.DAL;
using IMS.Presentation.HealthChecks;
using IMS.Presentation.Mapping;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using static IMS.Presentation.ApiConstants.Permissions;

namespace IMS.Presentation.Extensions;

public static class Extensions
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddBusinessLayerDependencies(configuration)
            .AddSwagger()
            .AddMapping()
            .AddAllHealthChecks()
            .AddValidation()
            .AddAuth0Authentication(configuration)
            .AddPolicies()
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

    private static IServiceCollection AddAuth0Authentication(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var domain = configuration["Auth0:Domain"];

                options.Authority = $"https://{domain}/";
                options.Audience = configuration["Auth0:Audience"];

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = $"https://{domain}/roles"
                };
            });

        return services;
    }

    private static IServiceCollection AddPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            var claimName = "permissions";
            
            options.AddPolicy(Users.Read, p => p.RequireClaim(claimName, Users.Read));
            options.AddPolicy(Users.Create, p => p.RequireClaim(claimName, Users.Create));
            options.AddPolicy(Users.Update, p => p.RequireClaim(claimName, Users.Update));

            options.AddPolicy(Tickets.Read, p => p.RequireClaim(claimName, Tickets.Read));
            options.AddPolicy(Tickets.Create, p => p.RequireClaim(claimName, Tickets.Create));
            options.AddPolicy(Tickets.Update, p => p.RequireClaim(claimName, Tickets.Update));

            options.AddPolicy(Boards.Read, p => p.RequireClaim(claimName, Boards.Read));
            options.AddPolicy(Boards.Create,p => p.RequireClaim(claimName, Boards.Create));
            options.AddPolicy(Boards.Update, p => p.RequireClaim(claimName, Boards.Update));

            options.AddPolicy(Internships.Read, p => p.RequireClaim(claimName, Internships.Read));
            options.AddPolicy(Internships.Create, p => p.RequireClaim(claimName, Internships.Create));
            options.AddPolicy(Internships.Update, p => p.RequireClaim(claimName, Internships.Update));

            options.AddPolicy(Feedbacks.Read, p => p.RequireClaim(claimName, Feedbacks.Read));
            options.AddPolicy(Feedbacks.Create, p => p.RequireClaim(claimName, Feedbacks.Create));
            options.AddPolicy(Feedbacks.Update, p => p.RequireClaim(claimName, Feedbacks.Update));
        });

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into the field",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
