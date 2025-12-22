using IMS.NotificationsCore.Services;
using MediatR;
using RecruitmentNotifications.Messages;

namespace Application.Interviews.Commands.ScheduleInterview;

public class ScheduleInterviewNotificationHandler(IMessageService sender) : INotificationHandler<InterviewScheduledEvent>
{
    public Task Handle(InterviewScheduledEvent notification, CancellationToken cancellationToken) =>
                sender.NotifyInterviewScheduled(notification, cancellationToken);
}
