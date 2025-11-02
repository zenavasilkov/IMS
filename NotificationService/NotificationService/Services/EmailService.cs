using FluentEmail.Core;
using FluentEmail.Core.Models;
using IMS.NotificationsCore.Messages;
using NotificationService.Services.Interfaces;

namespace NotificationService.Services;

public class EmailService(IFluentEmail emailSender, ILogger<EmailService> logger) : IEmailService
{
    public async Task Send<TEvent>(string email, string subject, string templatePath,
        TEvent eventModel, CancellationToken cancellationToken) where TEvent : BaseEvent
    {
        var templateFullPath = Path.Combine(AppContext.BaseDirectory, templatePath);

        if (!File.Exists(templateFullPath))
        {
            logger.LogError("Template file not found: {Path}", templateFullPath);
            return;
        }

        var response = await emailSender
            .To(email)
            .Subject(subject)
            .UsingTemplateFromFile(templateFullPath, eventModel)
            .SendAsync(cancellationToken);

        LogResponse(response, email);
    }

    private void LogResponse(SendResponse response, string email)
    {
        if (!response.Successful)
        {
            logger.LogError("Email sending to {Email} failed!", email);

            foreach (var error in response.ErrorMessages)
                logger.LogError("Error sending email to {Email}: {Error}", email, error);
        }
        else
        {
            logger.LogInformation("Email sent successfully to {Email}", email);
        }
    }
}
