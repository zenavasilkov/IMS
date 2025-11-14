namespace NotificationService.Constants;

public static class TemplatePaths
{
    private const string Base = "Templates/";

    public const string UserCreated = Base + "UserCreated.cshtml";
    public const string FeedbackCreated = Base + "FeedbackCreated.cshtml";
    public const string TicketCreated = Base + "TicketCreated.cshtml";
    public const string TicketStatusChanged = Base + "TicketStatusChanged.cshtml";

    public const string InterviewScheduled = Base + "InterviewScheduled.cshtml";
    public const string InterviewRescheduled = Base + "InterviewRescheduled.cshtml";
    public const string InterviewCancelled = Base + "InterviewCancelled.cshtml";
    public const string CandidatePassedToInternship = Base + "CandidatePassedToInternship.cshtml";
}
