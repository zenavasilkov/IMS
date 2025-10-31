using IMS.NotificationsCore.Options;
using IMS.NotificationsCore.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IMS.NotificationsCore.Extensions;

public static class ServiceExtensionCollections
{
    public static IServiceCollection AddNotifications(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));

        services.AddScoped<IMessageService, MessageService>();

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.UsingRabbitMq((context, rabbitMqConfigurator) =>
            {
                var rabbitMqOptions = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

                rabbitMqConfigurator.Host(rabbitMqOptions.HostName, rabbitMqOptions.VirtualHostName, configure =>
                {
                    configure.Username(rabbitMqOptions.UserName);
                    configure.Password(rabbitMqOptions.Password);
                });

                rabbitMqConfigurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
