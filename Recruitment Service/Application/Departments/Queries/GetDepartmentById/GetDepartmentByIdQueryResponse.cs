namespace Application.Departments.Queries.GetDepartmentById;

public sealed record GetDepartmentByIdQueryResponse(Guid Id, string Name, string? Description);
