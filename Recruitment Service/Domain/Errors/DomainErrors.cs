using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Errors;

public static class DomainErrors
{
    public static class CandidateErrors
    {
        public static readonly Error EmptyId = new($"{nameof(CandidateErrors)}.{nameof(EmptyId)}",
            "Candidate ID cannot be empty.");

        public static readonly Error EmptyFirstName = new($"{nameof(CandidateErrors)}.{nameof(EmptyFirstName)}",
            "Candidate first name is required.");

        public static readonly Error EmptyLastName = new($"{nameof(CandidateErrors)}.{nameof(EmptyLastName)}",
            "Candidate last name is required.");

        public static readonly Error InvalidEmail = new($"{nameof(CandidateErrors)}.{nameof(InvalidEmail)}",
            "Candidate email is invalid.");

        public static readonly Error AlreadyApplied = new($"{nameof(CandidateErrors)}.{nameof(AlreadyApplied)}",
            "Candidate is already applied.");

        public static readonly Error EmptyCvLink = new($"{nameof(CandidateErrors)}.{nameof(EmptyCvLink)}",
            "This candidate has no CV uploaded.");
    }

    public static class DepartmentErrors
    {
        public static readonly Error EmptyId = new($"{nameof(DepartmentErrors)}.{nameof(EmptyId)}",
            "Department ID cannot be empty.");

        public static readonly Error EmptyName =  new($"{nameof(DepartmentErrors)}.{nameof(EmptyName)}",
            "Department name is required.");

        public static readonly Error NameTooLong = new($"{nameof(DepartmentErrors)}.{nameof(NameTooLong)}",
                $"Department name cannot exceed {Department.MaxNameLength} characters.");

        public static readonly Error DescriptionTooLong =  new($"{nameof(DepartmentErrors)}.{nameof(DescriptionTooLong)}",
                $"Department description cannot exceed {Department.MaxDescriptionLength} characters.");
    }

    public static class EmployeeErrors
    {
        public static readonly Error EmptyId = new($"{nameof(EmployeeErrors)}.{nameof(EmptyId)}",
            "Employee ID cannot be empty.");

        public static readonly Error EmptyFirstName = new($"{nameof(EmployeeErrors)}.{nameof(EmptyFirstName)}",
            "Employee first name is required.");

        public static readonly Error EmptyLastName = new($"{nameof(EmployeeErrors)}.{nameof(EmptyLastName)}",
            "Employee last name is required.");

        public static readonly Error InvalidEmail =  new($"{nameof(EmployeeErrors)}.{nameof(InvalidEmail)}",
            "Employee email is invalid.");

        public static readonly Error InvalidRole = new($"{nameof(EmployeeErrors)}.{nameof(InvalidRole)}",
            "Invalid role specified");

        public static readonly Error NullDepartment = new($"{nameof(EmployeeErrors)}.{nameof(NullDepartment)}",
            "Employee must belong to a department.");

        public static readonly Error TheSameRole = new($"{nameof(EmployeeErrors)}.{nameof(TheSameRole)}",
            "New employee role has to deffer from current.");

        public static readonly Error UndefinedRole =  new($"{nameof(EmployeeErrors)}.{nameof(UndefinedRole)}",
            "Employee role cannot be undefined.");

        public static readonly Error SameDepartment = new($"{nameof(EmployeeErrors)}.{nameof(SameDepartment)}",
            "Employee already in this department.");

        public static readonly Error FirstNameTooLong = new($"{nameof(EmployeeErrors)}.{nameof(FirstNameTooLong)}",
            $"Employee first name cannot exceed {FullName.MaxLength} characters.");

        public static readonly Error LastNameTooLong = new($"{nameof(EmployeeErrors)}.{nameof(LastNameTooLong)}",
            $"Employee last name cannot exceed {FullName.MaxLength} characters.");

        public static readonly Error PatronymicTooLong = new($"{nameof(EmployeeErrors)}.{nameof(PatronymicTooLong)}",
            $"Employee first name cannot exceed {FullName.MaxLength} characters.");
    }

    public static class InterviewErrors
    {
        public static readonly Error EmptyId = new($"{nameof(InterviewErrors)}.{nameof(EmptyId)}",
            "Interview ID cannot be empty.");

        public static readonly Error NullCandidate = new($"{nameof(InterviewErrors)}.{nameof(NullCandidate)}",
            "Interview must have a candidate.");

        public static readonly Error NullInterviewer =  new($"{nameof(InterviewErrors)}.{nameof(NullInterviewer)}",
            "Interview must have an interviewer.");

        public static readonly Error NullDepartment = new($"{nameof(InterviewErrors)}.{nameof(NullDepartment)}",
            "Interview must belong to a department.");

        public static readonly Error ScheduledInPast = new($"{nameof(InterviewErrors)}.{nameof(ScheduledInPast)}",
            "Interview cannot be scheduled in the past.");

        public static readonly Error EmptyFeedback = new($"{nameof(InterviewErrors)}.{nameof(EmptyFeedback)}",
            "Interview feedback cannot be empty.");

        public static readonly Error CancelPassedInterview = new($"{nameof(InterviewErrors)}.{nameof(CancelPassedInterview)}",
            "Cannot cancel already passed interview.");

        public static readonly Error AlreadyCancelled =  new($"{nameof(InterviewErrors)}.{nameof(AlreadyCancelled)}",
            "Interview has already been cancelled.");

        public static readonly Error CannotAddFeedback = new($"{nameof(InterviewErrors)}.{nameof(CannotAddFeedback)}",
            "Cannot add a feedback to an interview that didn't take place");
    }
}
