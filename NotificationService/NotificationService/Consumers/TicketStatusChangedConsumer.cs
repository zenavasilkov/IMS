using IMS.NotificationsCore.Messages;
using MassTransit;
using NotificationService.Constants;
using NotificationService.Services.Interfaces;

namespace NotificationService.Consumers;

public class TicketStatusChangedConsumer(IEmailService emailService) : IConsumer<TicketStatusChangedEvent>
{
    public async Task Consume(ConsumeContext<TicketStatusChangedEvent> context)
    {
        var message = context.Message;

        string subject = string.Format(SubjectConstats.TicketStatusChanged, 
            message.Title, message.OldStatus, message.NewStatus);

        await emailService.Send(message.Email, subject, TemplatePaths.TicketStatusChanged, 
            message, context.CancellationToken);
    }
}
