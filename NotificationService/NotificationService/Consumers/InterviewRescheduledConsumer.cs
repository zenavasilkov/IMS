using MassTransit;
using NotificationService.Constants;
using NotificationService.Services.Interfaces;
using RecruitmentNotifications.Messages;

namespace NotificationService.Consumers;

public class InterviewRecheduledConsumer(IEmailService emailService) : IConsumer<InterviewRescheduledEvent>
{
    public async Task Consume(ConsumeContext<InterviewRescheduledEvent> context)
    {
        var message = context.Message;

        string subject = string.Format(SubjectConstats.InterviewRescheduled,
            message.ScheduledAt, message.RescheduledTo);

        await emailService.Send(message.CandidateEmail, subject,
            TemplatePaths.InterviewRescheduled, message, context.CancellationToken);

        await emailService.Send(message.InterviewerEmail, subject,
            TemplatePaths.InterviewRescheduled, message, context.CancellationToken);
    }
}
