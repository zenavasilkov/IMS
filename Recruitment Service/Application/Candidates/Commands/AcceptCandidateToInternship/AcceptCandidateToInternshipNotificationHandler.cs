using MediatR;
using RecruitmentNotifications.Messages;
using RecruitmentNotifications.Servises;

namespace Application.Candidates.Commands.AcceptCandidateToInternship;

internal class AcceptCandidateToInternshipNotificationHandler(IMessageSender sender) : INotificationHandler<CandidatePassedToInternshipEvent>
{
    public Task Handle(CandidatePassedToInternshipEvent notification, CancellationToken cancellationToken) =>
        sender.NotifyCandidatePassedToInterview(notification, cancellationToken);
}
