using Domain.Enums;
using Domain.Primitives;
using Domain.Shared;
using Domain.ValueObjects;
using static Domain.Errors.DomainErrors.EmployeeErrors;
using static Domain.ValueObjects.FullName;

namespace Domain.Entities;

public sealed class Employee : Entity
{
    private Employee(Guid id) : base(id) { }

    public Guid DepartmentId { get; private set; }
    public FullName FullName { get; private set; } = Default;
    public EmploeeRole Role { get; private set; }
    public string Email { get; private set; } = string.Empty;

    public Department? Department { get; private set; }

    public static Result<Employee> Create(Guid id, FullName fullName, EmploeeRole role, string email, Department department)
    {
        var result = ValidateImployee(id, role, email, department);

        if (result.IsFailure) return result.Error;

        var employee = new Employee(id)
        {
            FullName = fullName,
            Role = role,
            Email = email,
            Department = department,
            DepartmentId = department.Id
        };

        return employee;
    }

    public Result MoveTo(Department department)
    {
        if (department is null) return NullDepartment;

        if (department == Department) return SameDepartment;

        Department = department;
        DepartmentId = department.Id;

        return Result.Success();
    }

    public Result ChangeRole(EmploeeRole newRole)
    {
        if (newRole == Role) return TheSameRole;

        if(newRole == EmploeeRole.Undefined) return UndefinedRole;

        Role = newRole;

        return Result.Success();
    }

    public Result UpdateEmail(string newEmail)
    {
        if(!Validator.IsValidEmail(newEmail)) return InvalidEmail;

        Email = newEmail.Trim();

        return Result.Success();
    }

    private static Result ValidateImployee(Guid id, EmploeeRole role, string email, Department department)
    {
        if (!Enum.IsDefined(role)) return InvalidRole; 

        if (id == Guid.Empty) return EmptyId;

        if (!Validator.IsValidEmail(email)) return InvalidEmail;

        if (department is null) return NullDepartment;

        if (role == EmploeeRole.Undefined) return UndefinedRole;

        return Result.Success();
    }
 }
