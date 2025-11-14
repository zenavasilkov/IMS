using MassTransit;
using NotificationService.Constants;
using NotificationService.Services.Interfaces;
using RecruitmentNotifications.Messages;

namespace NotificationService.Consumers;

public class InterviewScheduledConsumer(IEmailService emailService) : IConsumer<InterviewScheduledEvent>
{
    public async Task Consume(ConsumeContext<InterviewScheduledEvent> context)
    {
        var message = context.Message;

        var subject = string.Format(SubjectConstats.InterviewScheduled, message.ScheduledAt);

        var sendToCandidate = emailService.Send(message.CandidateEmail, subject,
            TemplatePaths.InterviewScheduled, message, context.CancellationToken);

        var sendToInterviewer = emailService.Send(message.InterviewerEmail, subject,
            TemplatePaths.InterviewScheduled, message, context.CancellationToken);

        await Task.WhenAll(sendToCandidate, sendToInterviewer);
    }
}
