using IMS.NotificationsCore.Messages;
using MassTransit;
using NotificationService.Constants;
using NotificationService.Services.Interfaces;

namespace NotificationService.Consumers;

public class TicketCreatedConsumer(IEmailService emailService) : IConsumer<TicketCreatedEvent>
{
    public async Task Consume(ConsumeContext<TicketCreatedEvent> context)
    {
        var message = context.Message;

        await emailService.Send(message.Email, SubjectConstats.TicketCreated,
            TemplatePaths.TicketCreated, message, context.CancellationToken);
    }
}
