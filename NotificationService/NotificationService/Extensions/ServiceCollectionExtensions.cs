using MassTransit;
using Microsoft.Extensions.Options;
using NotificationService.Options;
using NotificationService.Services.Interfaces;
using NotificationService.Services;

namespace NotificationService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageBus(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.Configure<EmailSettingsOptions>(configuration.GetSection(EmailSettingsOptions.SectionName));
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));

        var emailSettingsOptions = configuration.GetSection(EmailSettingsOptions.SectionName).Get<EmailSettingsOptions>();

        services.AddFluentEmail(emailSettingsOptions!.DefaultUserName, emailSettingsOptions.Sender)
            .AddRazorRenderer()
            .AddSmtpSender(emailSettingsOptions.Host, emailSettingsOptions.Port);

        services.AddScoped<IEmailService, EmailService>();

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumers(typeof(ServiceCollectionExtensions).Assembly);
            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                var rabbitMqOptions = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
                configurator.Host(rabbitMqOptions.HostName, rabbitMqOptions.VirtualHostName, host =>
                {
                    host.Username(rabbitMqOptions.UserName);
                    host.Password(rabbitMqOptions.Password);
                });


                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
