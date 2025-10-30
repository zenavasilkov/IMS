namespace IMS.NotificationsCore.Messages;

public record BaseEvent
{
    DateTime SentAt { get; init; } = DateTime.UtcNow;
}
