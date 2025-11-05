using Domain.Enums;
using Domain.Shared;
using static Domain.Errors.DomainErrors;

namespace Domain.Entities;

public sealed class Employee : Entity
{
    private Employee(Guid id, string fullName, EmploeeRole role, string email, Department department) : base(id)
    {
        FullName = fullName;
        Role = role;
        Email = email;
        Department = department;
        DepartmentId = department.Id;
    }
    public Guid DepartmentId { get; private set; }
    public string FullName { get; private set; }
    public EmploeeRole Role { get; private set; }
    public string Email { get; private set; }

    public Department Department { get; private set; }

    public static Result<Employee> Create(Guid id, string fullName, EmploeeRole role, string email, Department department)
    {
        if (id == Guid.Empty)
            return Result.Failure<Employee>(EmployeeErrors.EmptyId);

        if (string.IsNullOrWhiteSpace(fullName))
            return Result.Failure<Employee>(EmployeeErrors.EmptyFullName);

        if (!Validator.IsValidEmail(email))
            return Result.Failure<Employee>(EmployeeErrors.InvalidEmail);

        if (department is null)
            return Result.Failure<Employee>(EmployeeErrors.NullDepartment);

        var employee = new Employee(id, fullName.Trim(), role, email.Trim(), department);

        return employee;
    }
}
