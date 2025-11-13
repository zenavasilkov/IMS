namespace NotificationService.Constants;

public static class SubjectConstats
{
    public const string UserCreated = "A new account has been created for you in the Intern Management System.";
    public const string FeedbackCreated = "You have received new feedback.";
    public const string TicketCreated = "A new assignment has been assigned to you in the Intern Management System.";
    public const string TicketStatusChanged = "The status of assignment '{0}' has changed from '{1}' to '{2}'.";

    public const string InterviewScheduled = "Your interview has been scheduled to {0}";
    public const string InterviewRescheduled = "Your interview has been rescheduled from {0}, to {1}";
    public const string InterviewCancelled = "Your Interview has been cancelled";
    public const string CandidatePassedToInternship = "Cangratulations you passed to internship";
}
