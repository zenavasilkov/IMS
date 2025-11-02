namespace NotificationService.Options;

public class EmailSettingsOptions
{
    public const string SectionName = "EmailSettings";

    public string Host { get; set; } = default!;
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public string DefaultUserName { get; set; } = default!;
    public string SenderEmailPassword { get; set; } = default!;
    public string Sender { get; set; } = default!;
}
