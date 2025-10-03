namespace IMS.DAL.Enums
{
    public enum Role
    {
        Unassigned,
        Admin,
        HRManager,
        Mentor,
        Intern
    }

    public enum TaskStatus
    { 
        Unassigned,
        ToDo,   
        InProgress,
        PullRequest,
        Done
    }

    public enum InternshipStatus
    {
        NotStarted,
        Ongoing,
        Completed,
        Cancelled
    }
}
