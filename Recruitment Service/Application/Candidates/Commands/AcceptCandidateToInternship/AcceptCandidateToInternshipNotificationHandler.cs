using MediatR;
using RecruitmentNotifications.Messages;
using IMS.NotificationsCore.Services;

namespace Application.Candidates.Commands.AcceptCandidateToInternship;

internal class AcceptCandidateToInternshipNotificationHandler(IMessageService sender) : INotificationHandler<CandidatePassedToInternshipEvent>
{
    public Task Handle(CandidatePassedToInternshipEvent notification, CancellationToken cancellationToken) =>
        sender.NotifyCandidatePassedToInterview(notification, cancellationToken);
}
