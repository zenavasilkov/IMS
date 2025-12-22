namespace IMS.NotificationsCore.Messages;

public record FeedbackCreatedEvent(
    string Comment,
    string Email) 
    : BaseEvent();
