using Domain.Entities;

namespace Domain.Errors;

public static class DomainErrors
{
    public static class CandidateErrors
    {
        public static readonly Error EmptyId =
            new($"{nameof(CandidateErrors)}.{nameof(EmptyId)}", "Candidate ID cannot be empty.");

        public static readonly Error EmptyFirstName =
            new($"{nameof(CandidateErrors)}.{nameof(EmptyFirstName)}", "Candidate first name is required.");

        public static readonly Error EmptyLastName =
            new($"{nameof(CandidateErrors)}.{nameof(EmptyLastName)}", "Candidate last name is required.");

        public static readonly Error InvalidEmail =
            new($"{nameof(CandidateErrors)}.{nameof(InvalidEmail)}", "Candidate email is invalid.");
    }

    public static class DepartmentErrors
    {
        public static readonly Error EmptyId =
            new($"{nameof(DepartmentErrors)}.{nameof(EmptyId)}", "DepartmentErrors ID cannot be empty.");

        public static readonly Error EmptyName =
            new($"{nameof(DepartmentErrors)}.{nameof(EmptyName)}", "DepartmentErrors name is required.");

        public static readonly Error NameTooLong =
            new($"{nameof(DepartmentErrors)}.{nameof(NameTooLong)}",
                $"DepartmentErrors name cannot exceed {Department._maxNameLength} characters.");

        public static readonly Error DescriptionTooLong =
            new($"{nameof(DepartmentErrors)}.{nameof(DescriptionTooLong)}",
                $"DepartmentErrors description cannot exceed {Department.maxDescriptionLength} characters.");
    }

    public static class EmployeeErrors
    {
        public static readonly Error EmptyId =
            new($"{nameof(EmployeeErrors)}.{nameof(EmptyId)}", "Employee ID cannot be empty.");

        public static readonly Error EmptyFullName =
            new($"{nameof(EmployeeErrors)}.{nameof(EmptyFullName)}", "Employee full name is required.");

        public static readonly Error InvalidEmail =
            new($"{nameof(EmployeeErrors)}.{nameof(InvalidEmail)}", "Employee email is invalid.");

        public static readonly Error NullDepartment =
            new($"{nameof(EmployeeErrors)}.{nameof(NullDepartment)}", "Employee must belong to a department.");
    }

    public static class InterviewErrors
    {
        public static readonly Error EmptyId =
           new($"{nameof(InterviewErrors)}.{nameof(EmptyId)}", "Interview ID cannot be empty.");

        public static readonly Error NullCandidate =
            new($"{nameof(InterviewErrors)}.{nameof(NullCandidate)}", "Interview must have a candidate.");

        public static readonly Error NullInterviewer =
            new($"{nameof(InterviewErrors)}.{nameof(NullInterviewer)}", "Interview must have an interviewer.");

        public static readonly Error NullDepartment =
            new($"{nameof(InterviewErrors)}.{nameof(NullDepartment)}", "Interview must belong to a department.");

        public static readonly Error ScheduledInPast =
           new($"{nameof(InterviewErrors)}.{nameof(ScheduledInPast)}", "Interview cannot be scheduled in the past.");

        public static readonly Error EmptyFeedback =
           new($"{nameof(InterviewErrors)}.{nameof(EmptyFeedback)}", "Interview feedback cannot be empty.");
    }
}
