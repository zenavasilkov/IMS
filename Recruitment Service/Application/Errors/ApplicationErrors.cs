using Domain.Entities;
using Domain.Errors;

namespace Application.Errors;

public static class ApplicationErrors
{
    public static class CandidateErrors
    {
        public static readonly Error NotFound = new($"{nameof(CandidateErrors)}.{NotFound}", "Candidate was not found");

        public static readonly Error IdIsNotUnique = new($"{nameof(CandidateErrors)}.{IdIsNotUnique}", "Candidate with given ID already exists");

        public static readonly Error EmailIsNotUnique =
            new($"{nameof(CandidateErrors)}.{EmailIsNotUnique}", "Candidate with given email already registered");
    }

    public static class DepartmentErrors
    {
        public static readonly Error NotFound = new($"{nameof(DepartmentErrors)}.{NotFound}", "Department was not found");

        public static readonly Error IdIsNotUnique = new($"{nameof(DepartmentErrors)}.{IdIsNotUnique}", "Department with given ID already exists");

        public static readonly Error NameIsNotUnique = new($"{nameof(DepartmentErrors)}.{NameIsNotUnique}", "Department with given name already exists");
    }

    public static class EmployeeErrors
    {
        public static readonly Error NotFound = new($"{nameof(EmployeeErrors)}.{NotFound}", "Employee was not found");

        public static readonly Error IdIsNotUnique = new($"{nameof(EmployeeErrors)}.{IdIsNotUnique}", "Employee with given ID already exists");

        public static readonly Error EmailIsNotUnique = new($"{nameof(EmployeeErrors)}.{EmailIsNotUnique}", "Employee with given email already registered");
    }

    public static class InterviewErrors
    {
        public static readonly Error NotFound = new($"{nameof(InterviewErrors)}.{NotFound}", "Interview was not found");
    }
}
