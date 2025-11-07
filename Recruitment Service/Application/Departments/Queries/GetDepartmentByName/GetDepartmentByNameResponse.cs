namespace Application.Departments.Queries.GetDepartmentByName;

public sealed record GetDepartmentByNameResponse(Guid Id, string Name, string? Description);
