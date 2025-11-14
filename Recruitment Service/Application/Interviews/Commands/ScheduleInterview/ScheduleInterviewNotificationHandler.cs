using MediatR;
using RecruitmentNotifications.Messages;
using RecruitmentNotifications.Servises;

namespace Application.Interviews.Commands.ScheduleInterview;

public class ScheduleInterviewNotificationHandler(IMessageSender sender) : INotificationHandler<InterviewScheduledEvent>
{
    public Task Handle(InterviewScheduledEvent notification, CancellationToken cancellationToken) =>
                sender.NotifyInterviewScheduled(notification, cancellationToken);
}
