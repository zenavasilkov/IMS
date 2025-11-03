using IMS.NotificationsCore.Messages;
using MassTransit;
using NotificationService.Constants;
using NotificationService.Services.Interfaces;

namespace NotificationService.Consumers;

public class FeedbackCreatedConsumer(IEmailService emailService) : IConsumer<FeedbackCreatedEvent>
{
    public async Task Consume(ConsumeContext<FeedbackCreatedEvent> context)
    {
        var message = context.Message;

        await emailService.Send(message.Email, SubjectConstats.FeedbackCreated, 
            TemplatePaths.FeedbackCreated, message, context.CancellationToken);
    }
}
