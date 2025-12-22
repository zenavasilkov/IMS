namespace IMS.NotificationsCore.Messages;

public record TicketCreatedEvent(
    string Title,
    string Description,
    DateTime Deadline,
    string Email) : BaseEvent;
