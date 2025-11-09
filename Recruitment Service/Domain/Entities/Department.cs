using Domain.Primitives;
using Domain.Shared;
using static Domain.Errors.DomainErrors;

namespace Domain.Entities;

public sealed class Department : Entity
{
    public const int MaxDescriptionLength = 1000;
    public const int MaxNameLength = 100;

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

        if (name.Length > MaxNameLength) return DepartmentErrors.NameTooLong;

        if (description is not null && description.Length > MaxDescriptionLength)
            return DepartmentErrors.DescriptionTooLong;

        var department = new Department(id, name, description?.Trim());

        return department;
    }

    public Result Rename(string newName)
    {
        newName = newName.Trim();

        if (!string.IsNullOrWhiteSpace(newName)) return DepartmentErrors.EmptyName;

        if (newName.Length > MaxNameLength) return DepartmentErrors.NameTooLong;

        Name = newName;

        return Result.Success();
    }

    public Result UpdateDescription(string? newDescription)
    {
        newDescription = newDescription?.Trim();

        if (newDescription is not null && newDescription.Length > MaxDescriptionLength)
            return DepartmentErrors.DescriptionTooLong;

        Description = newDescription;

        return Result.Success();
    }
}
