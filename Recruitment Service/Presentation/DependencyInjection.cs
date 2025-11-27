using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using static Presentation.ApiConstants.Permissions;
﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Presentation.ApiConstants.ApiConstants;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuth0Authentication(configuration)
            .AddPolicies()
            .AddSwaggerGen();
        
        return services;
    }
    
    
    private static IServiceCollection AddAuth0Authentication(this IServiceCollection services, IConfiguration configuration)
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
            
            options.AddPolicy(CandidatesPermissions.Read, p =>
                p.RequireClaim(claimName, CandidatesPermissions.Read));
            options.AddPolicy(CandidatesPermissions.Register, p =>
                p.RequireClaim(claimName, CandidatesPermissions.Register));
            options.AddPolicy(CandidatesPermissions.AcceptToInternship, p =>
                p.RequireClaim(claimName, CandidatesPermissions.AcceptToInternship));
            options.AddPolicy(CandidatesPermissions.ManageCandidates, p =>
                p.RequireClaim(claimName, CandidatesPermissions.ManageCandidates));
            
            options.AddPolicy(EmployeesPermissions.Read, p =>
                p.RequireClaim(claimName, EmployeesPermissions.Read));
            options.AddPolicy(EmployeesPermissions.ManageEmployees, p =>
                p.RequireClaim(claimName, EmployeesPermissions.ManageEmployees));

            options.AddPolicy(DepartmentsPermissions.Read, p =>
                p.RequireClaim(claimName, DepartmentsPermissions.Read));
            options.AddPolicy(DepartmentsPermissions.ManageDepartments,p =>
                p.RequireClaim(claimName, DepartmentsPermissions.ManageDepartments));
            
            
            options.AddPolicy(InterviewsPermissions.Read, p =>
                p.RequireClaim(claimName, InterviewsPermissions.Read));
            options.AddPolicy(InterviewsPermissions.AddFeedbacks, p =>
                p.RequireClaim(claimName, InterviewsPermissions.AddFeedbacks));
            options.AddPolicy(InterviewsPermissions.ManageInterviews, p =>
                p.RequireClaim(claimName, InterviewsPermissions.ManageInterviews));
        });

        return services;
    }

    private static IServiceCollection AddSwaggerGen(this IServiceCollection services)
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
        return services.SetCors(configuration);
    }
    
    private static IServiceCollection SetCors(this IServiceCollection services, IConfiguration configuration)
    {
        var frontendUrl = configuration["FrontendUrl"] ?? "http://localhost:5173";
        
        services.AddCors(options =>
        {
            options.AddPolicy(AllowFrontend, policy =>
            {
                policy
                    .WithOrigins(frontendUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }
}
