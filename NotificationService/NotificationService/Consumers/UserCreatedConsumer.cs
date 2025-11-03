using MassTransit;
using NotificationService.Services.Interfaces;
using IMS.NotificationsCore.Messages;
using NotificationService.Constants;

namespace NotificationService.Consumers;

public class UserCreatedConsumer(IEmailService emailService) : IConsumer<UserCreatedEvent>
{
    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var message = context.Message;

        await emailService.Send(message.Email, SubjectConstats.UserCreated,
            TemplatePaths.UserCreated, message, context.CancellationToken);
    }
}
