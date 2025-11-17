using IMS.NotificationsCore.Messages;
using RecruitmentNotifications.Messages;

namespace IMS.NotificationsCore.Services;

public interface IMessageService
{
    Task NotifyUserCreated(UserCreatedEvent message, CancellationToken cancellationToken = default);
    Task NotifyTicketStatusChanged(TicketStatusChangedEvent message, CancellationToken cancellationToken = default);
    Task NotifyTicketCreated(TicketCreatedEvent message, CancellationToken cancellationToken = default);
    Task NotifyFeedbackCreated(FeedbackCreatedEvent message, CancellationToken cancellationToken = default);

    Task NotifyInterviewScheduled(InterviewScheduledEvent message, CancellationToken cancellationToken = default);
    Task NotifyInterviewRescheduled(InterviewRescheduledEvent message, CancellationToken cancellationToken = default);
    Task NotifyInterviewCancelled(InterviewCancelledEvent message, CancellationToken cancellationToken = default);
    Task NotifyCandidatePassedToInterview(CandidatePassedToInternshipEvent message, CancellationToken cancellationToken = default);
}
