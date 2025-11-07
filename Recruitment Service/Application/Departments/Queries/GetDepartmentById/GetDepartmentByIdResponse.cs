namespace Application.Departments.Queries.GetDepartmentById;

public sealed record GetDepartmentByIdResponse(Guid Id, string Name, string? Description);
