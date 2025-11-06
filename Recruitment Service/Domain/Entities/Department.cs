using Domain.Shared;
using static Domain.Errors.DomainErrors;

namespace Domain.Entities;

public sealed class Department : Entity
{
    public const int maxDescriptionLength = 1000;
    public const int maxNameLength = 100;

    private readonly List<Employee> _employees = [];

    public IReadOnlyCollection<Employee> Employees => _employees;

    private Department(Guid id, string name, string? description = null) : base(id)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; private set; }
    public string? Description { get; private set; }

    public static Result<Department> Create(Guid id, string name, string? description = null)
    {
        if (id == Guid.Empty) return DepartmentErrors.EmptyId;

        if (string.IsNullOrWhiteSpace(name)) return DepartmentErrors.EmptyName;

        name = name.Trim();

        if (name.Length > maxNameLength) return DepartmentErrors.NameTooLong;

        if (description is not null && description.Length > maxDescriptionLength)
            return DepartmentErrors.DescriptionTooLong;

        var department = new Department(id, name, description?.Trim());

        return department;
    }

    public Result Rename(string newName)
    {
        newName = newName.Trim();

        if (!string.IsNullOrWhiteSpace(newName)) return DepartmentErrors.EmptyName;

        if (newName.Length > maxNameLength) return DepartmentErrors.NameTooLong;

        Name = newName;

        return Result.Success();
    }

    public Result UpdateDescription(string? newDescription)
    {
        newDescription = newDescription?.Trim();

        if (newDescription is not null && newDescription.Length > maxDescriptionLength)
            return DepartmentErrors.DescriptionTooLong;

        Description = newDescription;

        return Result.Success();
    }

    public Result AddEmployee(Employee employee)
    {
        if (employee is null) return DepartmentErrors.AddEmptyEmployee;

        if (employee.Department is not null && employee.Department != this)
            return DepartmentErrors.AlreadyHasDepartment;

        _employees.Add(employee);

        return Result.Success();
    }

    public Result RemoveEmployee(Guid id)
    {
        var employee = _employees.FirstOrDefault(x => x.Id == id);

        if (employee is null) return DepartmentErrors.EmployeeNotInTheDepartment;

        _employees.Remove(employee);

        return Result.Success();
    }
}
