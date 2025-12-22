using IMS.NotificationsCore.Services;
using MediatR;
using RecruitmentNotifications.Messages;

namespace Application.Interviews.Commands.CancelInterview;

public class CancelInterviewNotificationHandler(IMessageService sender) : INotificationHandler<InterviewCancelledEvent>
{
    public Task Handle(InterviewCancelledEvent notification, CancellationToken cancellationToken) =>
        sender.NotifyInterviewCancelled(notification, cancellationToken);
}
