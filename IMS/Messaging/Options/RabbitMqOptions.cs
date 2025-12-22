namespace IMS.NotificationsCore.Options;

public class RabbitMqOptions
{
    public const string SectionName = "RabbitMQ";

    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string HostName { get; set; } = default!;
    public string VirtualHostName { get; set; } = default!;
}
