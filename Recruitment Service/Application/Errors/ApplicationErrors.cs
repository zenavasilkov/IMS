using Domain.Entities;
using Domain.Errors;

namespace Application.Errors;

public static class ApplicationErrors
{
    public static class CandidateErrors
    {
        public static readonly Error NotFound = new($"{nameof(CandidateErrors)}.{nameof(NotFound)}",
            "Candidate was not found");

        public static readonly Error IdIsNotUnique = new($"{nameof(CandidateErrors)}.{nameof(IdIsNotUnique)}",
            "Candidate with given ID already exists");

        public static readonly Error EmailIsNotUnique = new($"{nameof(CandidateErrors)}.{nameof(EmailIsNotUnique)}",
            "Candidate with given email already registered");
    }

    public static class DepartmentErrors
    {
        public static readonly Error NotFound = new($"{nameof(DepartmentErrors)}.{nameof(NotFound)}",
            "Department was not found");

        public static readonly Error IdIsNotUnique = new($"{nameof(DepartmentErrors)}.{nameof(IdIsNotUnique)}",
            "Department with given ID already exists");

        public static readonly Error NameIsNotUnique = new($"{nameof(DepartmentErrors)}.{nameof(NameIsNotUnique)}",
            "Department with given name already exists");
    }

    public static class EmployeeErrors
    {
        public static readonly Error NotFound = new($"{nameof(EmployeeErrors)}.{nameof(NotFound)}",
            "Employee was not found");

        public static readonly Error IdIsNotUnique = new($"{nameof(EmployeeErrors)}.{nameof(IdIsNotUnique)}",
            "Employee with given ID already exists");

        public static readonly Error EmailIsNotUnique = new($"{nameof(EmployeeErrors)}.{nameof(EmailIsNotUnique)}",
            "Employee with given email already registered");
    }

    public static class InterviewErrors
    {
        public static readonly Error NotFound = new($"{nameof(InterviewErrors)}.{nameof(NotFound)}",
            "Interview was not found");
    }
}
