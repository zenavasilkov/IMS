namespace NotificationService.Constants;

public static class SubjectConstats
{
    public const string UserCreated = "You have new account in IMS";
    public const string FeedbackCreated = "You recieved feedback on assigment '{TicketTitle}'";
    public const string TicketCreated = "You have new assignment in IMS";
    public const string TicketStatusChanged = "Assignment '{TicketTitle}' status has been changed from '{OldStatust} to '{NewStatus}'";
}
