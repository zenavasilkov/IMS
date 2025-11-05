using Domain.Errors;
using Domain.Shared;

namespace Domain.Entities;

public sealed class Department : Entity
{
    public const int maxDescriptionLength = 1000;
    public const int _maxNameLength = 100;

    private Department(Guid id, string name, string? description = null) : base(id)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; private set; }
    public string? Description { get; private set; }

    public static Result<Department> Create(Guid id, string name, string? description = null)
    {
        if (id == Guid.Empty)
            return Result.Failure<Department>(DomainErrors.DepartmentErrors.EmptyId);

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Department>(DomainErrors.DepartmentErrors.EmptyName);

        name = name.Trim();

        if (name.Length > _maxNameLength)
            return Result.Failure<Department>(DomainErrors.DepartmentErrors.NameTooLong);

        if (description is not null && description.Length > maxDescriptionLength)
            return Result.Failure<Department>(DomainErrors.DepartmentErrors.DescriptionTooLong);

        var department = new Department(id, name, description?.Trim());

        return department;
    }
}
