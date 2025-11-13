using IMS.NotificationsCore.Messages;

namespace NotificationService.Services.Interfaces;

public interface IEmailService
{
    Task Send<TEvent>(string email, string subject, string templatePath, 
        TEvent eventModel, CancellationToken cancellationToken);
}
