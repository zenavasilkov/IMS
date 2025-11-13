using MediatR;
using RecruitmentNotifications.Messages;
using RecruitmentNotifications.Servises;

namespace Application.Interviews.Commands.CancelInterview;

public class CancelInterviewNotificationHandler(IMessageSender sender) : INotificationHandler<InterviewCancelledEvent>
{
    public Task Handle(InterviewCancelledEvent notification, CancellationToken cancellationToken) =>
        sender.NotifyInterviewCancelled(notification, cancellationToken);
}
