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

    private Employee() : base(Guid.Empty) { }

    public Guid DepartmentId { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public EmploeeRole Role { get; private set; }
    public string Email { get; private set; } = string.Empty;

    public Department? Department { get; private set; }

    public static Result<Employee> Create(Guid id, string fullName, EmploeeRole role, string email, Department department)
    {
        if (id == Guid.Empty) return EmployeeErrors.EmptyId;

        if (string.IsNullOrWhiteSpace(fullName)) return EmployeeErrors.EmptyFullName;

        if (!Validator.IsValidEmail(email)) return EmployeeErrors.InvalidEmail;

        if (department is null) return EmployeeErrors.NullDepartment;

        if(role == EmploeeRole.Undefined) return EmployeeErrors.UndefinedRole;

        var employee = new Employee(id, fullName.Trim(), role, email.Trim(), department);

        return employee;
    }

    public Result MoveTo(Department department)
    {
        if (department is null) return EmployeeErrors.NullDepartment;

        Department = department;
        DepartmentId = department.Id;

        return Result.Success();
    }

    public Result Promote(EmploeeRole newRole)
    {
        if (newRole == Role) return EmployeeErrors.TheSameRole;

        if(newRole == EmploeeRole.Undefined) return EmployeeErrors.UndefinedRole;

        Role = newRole;

        return Result.Success();
    }

    public Result UpdateEmail(string newEmail)
    {
        if(!Validator.IsValidEmail(newEmail)) return EmployeeErrors.InvalidEmail;

        Email = newEmail.Trim();

        return Result.Success();
    }
}
