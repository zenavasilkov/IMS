using MassTransit;

namespace NotificationService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageBus(
        this IServiceCollection services, 
        IConfiguration configuration, 
        string queueName = "user-created-queue")
    {
        services.AddMassTransit(x =>
        {
            //TODO: Add Consumers here

            x.UsingRabbitMq((ctx, cfg) =>
            {
                var rabbitHost = configuration.GetValue<string>("RabbitMQ:Host") ?? "rabbitmq://localhost";
                var username = configuration.GetValue<string>("RabbitMQ:Username") ?? "guest";
                var password = configuration.GetValue<string>("RabbitMQ:Password") ?? "guest";

                cfg.Host(rabbitHost, h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

                cfg.ReceiveEndpoint(queueName, e =>
                {
                    //TODO: Configure Consumers here
                });
            });
        });

        return services;
    }
}
