using MassTransit;
using NotificationService.Constants;
using NotificationService.Services.Interfaces;
using RecruitmentNotifications.Messages;

namespace NotificationService.Consumers;

public class CandidatePassedToInternshipConsumer(IEmailService emailService) : IConsumer<CandidatePassedToInternshipEvent>
{
    public async Task Consume(ConsumeContext<CandidatePassedToInternshipEvent> context)
    {
        var message = context.Message;

        await emailService.Send(message.Email, SubjectConstats.CandidatePassedToInternship,
            TemplatePaths.CandidatePassedToInternship, message, context.CancellationToken); 
    }
}
