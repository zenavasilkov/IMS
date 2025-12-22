using MediatR;

namespace IMS.NotificationsCore.Messages;

public record BaseEvent : INotification
{
    DateTime SentAt { get; init; } = DateTime.UtcNow;
}
