namespace IMS.Messaging.Messaging;

public record BaseEvent
{
    DateTime SentAt { get; init; } = DateTime.UtcNow;
}
