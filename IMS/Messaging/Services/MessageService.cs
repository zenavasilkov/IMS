using IMS.NotificationsCore.Messages;
using MassTransit;
using RecruitmentNotifications.Messages;

namespace IMS.NotificationsCore.Services;

public class MessageService(IPublishEndpoint publishEndpoint) : IMessageService
{
    public Task NotifyFeedbackCreated(FeedbackCreatedEvent message,
        CancellationToken cancellationToken = default) => publishEndpoint.Publish(message, cancellationToken);

    public Task NotifyTicketCreated(TicketCreatedEvent message, CancellationToken cancellationToken = default)
        => publishEndpoint.Publish(message, cancellationToken);

    public Task NotifyTicketStatusChanged(TicketStatusChangedEvent message,
        CancellationToken cancellationToken = default) => publishEndpoint.Publish(message, cancellationToken);

    public Task NotifyUserCreated(UserCreatedEvent message,
        CancellationToken cancellationToken = default) => publishEndpoint.Publish(message, cancellationToken); 

    public Task NotifyCandidatePassedToInterview(CandidatePassedToInternshipEvent message,
        CancellationToken cancellationToken = default) => publishEndpoint.Publish(message, cancellationToken);

    public Task NotifyInterviewCancelled(InterviewCancelledEvent message,
        CancellationToken cancellationToken = default) => publishEndpoint.Publish(message, cancellationToken);

    public Task NotifyInterviewRescheduled(InterviewRescheduledEvent message,
        CancellationToken cancellationToken = default) => publishEndpoint.Publish(message, cancellationToken);

    public Task NotifyInterviewScheduled(InterviewScheduledEvent message,
        CancellationToken cancellationToken = default) => publishEndpoint.Publish(message, cancellationToken);
}
