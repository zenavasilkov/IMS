using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Quartz;
using Testcontainers.PostgreSql;

namespace IMS.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .WithDatabase("postgres")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Tests");
        
        builder.ConfigureServices(services =>
        {
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });
 
            services.AddSingleton<IAuthorizationPolicyProvider, AllowAllPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, AllowAnonymousAuthorizationHandler>();
 
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            });
            
            var descriptorsToRemove = services.Where(d =>
                d.ServiceType == typeof(DbContextOptions<ImsDbContext>) ||
                d.ServiceType == typeof(ImsDbContext) ||
                d.ServiceType.Name.Contains("DatabaseProvider") ||
                d.ServiceType.Name.Contains("DbContextOptions") ||
                d.ServiceType.Name.Contains("IDbContextOptionsConfiguration")
                ).ToList();

            foreach (var descriptor in descriptorsToRemove)
                services.Remove(descriptor);
            
            services.RemoveAll<ISchedulerFactory>();
            services.RemoveAll<Quartz.Spi.IJobFactory>();
            services.RemoveAll<IHostedService>();
            
            services.AddSingleton<IAuthorizationHandler, AllowAnonymousAuthorizationHandler>();

            services.AddDbContext<ImsDbContext>(options => options.UseNpgsql(_dbContainer.GetConnectionString()));
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ImsDbContext>();

        await db.Database.MigrateAsync();
    }

    Task IAsyncLifetime.DisposeAsync() => _dbContainer.StopAsync();
}

public class AllowAnonymousAuthorizationHandler : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        foreach (var req in context.PendingRequirements.ToList())
            context.Succeed(req);

        return Task.CompletedTask;
    }
}
