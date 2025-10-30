using IMS.Messaging.Messaging;

namespace IMS.Messaging.Services;

public interface IMessageService
{
    Task NotifyUserCreated(UserCreatedEvent userCreatedEvent, CancellationToken cancellationToken = default);

    Task NotifyTicketStatusChanged(TicketStatusChangedEvent ticketStatusChangedEvent, CancellationToken cancellationToken = default);

    Task NotifyTicketCreated(TicketCreatedEvent ticketCreatedEvent, CancellationToken cancellationToken = default);

    Task NotifyFeedbackCreated(FeedbackCreatedEvent feedbackCreatedEvent, CancellationToken cancellationToken = default);
}
