using Domain.Contracts.Repositories;
using Infrastructure.BackgroundJobs;
using Infrastructure.Interceptors;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure;

public static class DependencyInjection
{
    private const int OutboxProcessingIntervalInSeconds = 10;

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext(configuration)
            .AddInterceptors()
            .AddOutboxProcessor()
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped(typeof(IGenericReadOnlyRepository<>), typeof(GenericReadOnlyRepository<>))
            .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddScoped<IDepartmentRepository, DepartmentRepository>()
            .AddScoped<ICandidateRepository, CandidateRepository>()
            .AddScoped<IInterviewRepository, InterviewRepository>()
            .AddScoped<IEmployeeRepository, EmployeeRepository>();

        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<RecruitmentDbContext>((sp, options) => {

            var interceptor = sp.GetRequiredService<ConvertDomainEventsToOutboxMessageInterceptor>();

            options.UseNpgsql(connectionString)
            .AddInterceptors(interceptor);
        });

        return services;
    }

    private static IServiceCollection AddInterceptors(this IServiceCollection services)
    { 

        services.AddSingleton<ConvertDomainEventsToOutboxMessageInterceptor>();
        return services;
    }

    private static IServiceCollection AddOutboxProcessor(this IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(opts => opts.WithIdentity(jobKey))
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                    .WithSimpleSchedule(schedule => schedule
                        .WithIntervalInSeconds(OutboxProcessingIntervalInSeconds)
                        .RepeatForever()));
        });

        services.AddQuartzHostedService();

        return services;
    }

    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<RecruitmentDbContext>();

        if (dbContext.Database.IsRelational())
        {
            dbContext.Database.Migrate();
        }
    }
}
