using MassTransit;
using NotificationService.Constants;
using NotificationService.Services.Interfaces;
using RecruitmentNotifications.Messages;

namespace NotificationService.Consumers;

public class InterviewCancelledConsumer(IEmailService emailService) : IConsumer<InterviewCancelledEvent>
{
    public async Task Consume(ConsumeContext<InterviewCancelledEvent> context)
    {
        var message = context.Message;

        await emailService.Send(message.CandidateEmail, SubjectConstats.InterviewCancelled,
            TemplatePaths.InterviewCancelled, message, context.CancellationToken);

        await emailService.Send(message.InterviewerEmail, SubjectConstats.InterviewCancelled,
            TemplatePaths.InterviewCancelled, message, context.CancellationToken);
    }
}
