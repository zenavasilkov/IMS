namespace Presentation.ApiRoutes;

public static class ApiRoutes
{
    public const string Id = "{id:guid}";

    public const string api = "api";

    public static class Candidates
    {
        public const string Base = $"{api}/candidates";

        public const string ByEmail = $"{Base}/by-email/{{email}}";
        public const string AcceptToInternship = $"{Base}/accept-to-internship";
        public const string UpdateCvLink = $"{Base}/update-cv-link";
        public const string Register = $"{Base}/register";
        public const string GetAll = $"{Base}/get-all";
    }

    public static class Departments
    {
        public const string Base = $"{api}/departments";

        public const string Rename = $"{Base}/rename";
        public const string UpdateDescription = $"{Base}/update-description";
        public const string ByName = $"{Base}/by-name/{{name}}";
        public const string GetAll = $"{Base}/get-all";
    }

    public static class Employees
    {
        public const string Base = $"{api}/employees";

        public const string ChangeRole = $"{Base}/change-role";
        public const string MoveToDepartment = $"{Base}/move-to-department";
        public const string GetAll = $"{Base}/get-all";
    }

    public static class Interviews
    {
        public const string Base = $"{api}/interviews";

        public const string Reschedule = $"{Base}/reschedule";
        public const string Cancel = $"{Base}/cancel";
        public const string AddFeedback = $"{Base}/add-feedback";
        public const string ByCandidateId = $"{Base}/by-candidate";
        public const string GetAll = $"{Base}/get-all";
    }
}
