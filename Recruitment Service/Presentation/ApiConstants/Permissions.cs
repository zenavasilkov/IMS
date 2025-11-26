namespace Presentation.ApiConstants;

public static class Permissions
{
    public static class CandidatesPermissions
    {
        public const string Read = "read:candidates";
        public const string Register = "register:candidates";
        public const string AcceptToInternship = "accept:candidates";
        public const string ManageCandidates = "manage:candidates";
    }
    
    public static class EmployeesPermissions
    {
        public const string Read = "read:employees";
        public const string ManageEmployees = "manage:employees";
    }

    public static class DepartmentsPermissions
    {
        public const string Read = "read:departments";
        public const string ManageDepartments = "manage:departments";
    }

    public static class InterviewsPermissions
    {
        public const string Read = "read:interviews";
        public const string ManageInterviews = "manage:interviews";
        public const string AddFeedbacks = "add:feedbacks";
    }
}
