namespace IMS.NotificationsCore.Messages;

public record TicketStatusChangedEvent(
    string Title,
    string Description,
    string OldStatus,
    string NewStatus,
    string Email) : BaseEvent;
