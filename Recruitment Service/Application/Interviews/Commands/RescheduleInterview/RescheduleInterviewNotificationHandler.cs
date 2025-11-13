using MediatR;
using RecruitmentNotifications.Messages;
using RecruitmentNotifications.Servises;

namespace Application.Interviews.Commands.RescheduleInterview;

public class RescheduleInterviewNotificationHandler(IMessageSender sender) : INotificationHandler<InterviewRescheduledEvent>
{
    public Task Handle(InterviewRescheduledEvent notification, CancellationToken cancellationToken) =>
        sender.NotifyInterviewRescheduled(notification, cancellationToken);
}
