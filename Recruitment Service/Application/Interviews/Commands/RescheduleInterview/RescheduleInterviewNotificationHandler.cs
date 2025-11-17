using IMS.NotificationsCore.Services;
using MediatR;
using RecruitmentNotifications.Messages;

namespace Application.Interviews.Commands.RescheduleInterview;

public class RescheduleInterviewNotificationHandler(IMessageService sender) : INotificationHandler<InterviewRescheduledEvent>
{
    public Task Handle(InterviewRescheduledEvent notification, CancellationToken cancellationToken) =>
        sender.NotifyInterviewRescheduled(notification, cancellationToken);
}
