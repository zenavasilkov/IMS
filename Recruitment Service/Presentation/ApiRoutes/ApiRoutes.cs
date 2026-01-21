namespace Presentation.ApiRoutes;

public static class ApiRoutes
{
    public const string Id = "{id:guid}";

    private const string Api = "api";

    public static class Candidates
    {
        public const string Base = $"{Api}/candidates";

        public const string ByEmail = "by-email/{email}";
        public const string AcceptToInternship = "accept-to-internship";
        public const string UpdateCv = $"{Id}/cv";
        public const string Register = "register";
        public const string GetAll = "get-all";
        public const string GetCvUrl = $"{Id}/cv";
    }

    public static class Departments
    {
        public const string Base = $"{Api}/departments";

        public const string Rename = "rename";
        public const string UpdateDescription = "update-description";
        public const string ByName = "by-name/{{name}}";
        public const string GetAll = "get-all";
    }

    public static class Employees
    {
        public const string Base = $"{Api}/employees";

        public const string ChangeRole = "change-role";
        public const string MoveToDepartment = "move-to-department";
        public const string GetAll = "get-all";
    }

    public static class Interviews
    {
        public const string Base = $"{Api}/interviews";

        public const string Reschedule = "reschedule";
        public const string Cancel = "cancel";
        public const string AddFeedback = "add-feedback";
        public const string ByCandidateId = "by-candidate";
        public const string GetAll = "get-all";
    }
}
