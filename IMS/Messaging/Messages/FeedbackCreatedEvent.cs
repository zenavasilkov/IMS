namespace IMS.Messaging.Messaging;

public record FeedbackCreatedEvent(
    string Comment,
    string Email) 
    : BaseEvent();
