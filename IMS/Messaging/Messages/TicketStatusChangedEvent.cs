namespace IMS.Messaging.Messaging;

public record TicketStatusChangedEvent(
    string Title,
    string Description,
    string OldStatus,
    string NewStatus,
    string Email) : BaseEvent;
