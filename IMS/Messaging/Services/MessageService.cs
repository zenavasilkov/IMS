using IMS.Messaging.Messaging;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace IMS.Messaging.Services;

public class MessageService(IPublishEndpoint publishEndpoint, ILogger<MessageService> logger) : IMessageService
{
    public Task NotifyFeedbackCreated(FeedbackCreatedEvent feedbackCreatedEvent, CancellationToken cancellationToken = default)
    {
        var notification = publishEndpoint.Publish(feedbackCreatedEvent, cancellationToken);

        logger.LogInformation("Notification of feebback sent to Email : {Email}", feedbackCreatedEvent.Email);

        return notification;
    }

    public Task NotifyTicketCreated(TicketCreatedEvent ticketCreatedEvent, CancellationToken cancellationToken = default)
    {
        var notification = publishEndpoint.Publish(ticketCreatedEvent, cancellationToken);

        logger.LogInformation("Notification of ticket creation sent to Email : {Email}", ticketCreatedEvent.Email);

        return notification;
    }

    public Task NotifyTicketStatusChanged(TicketStatusChangedEvent ticketStatusChangedEvent, CancellationToken cancellationToken = default)
    {
        var notification = publishEndpoint.Publish(ticketStatusChangedEvent, cancellationToken);

        logger.LogInformation("Notification of changing ticket status sent to Email : {Email}", ticketStatusChangedEvent.Email);

        return notification;
    }

    public Task NotifyUserCreated(UserCreatedEvent userCreatedEvent, CancellationToken cancellationToken = default)
    {
        var notification = publishEndpoint.Publish(userCreatedEvent, cancellationToken);

        logger.LogInformation("Notification of user creation sent to Email : {Email}", userCreatedEvent.Email);

        return notification;
    }
}
