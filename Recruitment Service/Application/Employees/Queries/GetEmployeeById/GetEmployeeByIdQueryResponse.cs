using Domain.Enums;

namespace Application.Employees.Queries.GetEmployeeById;

public sealed record GetEmployeeByIdQueryResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string? Patronymic,
    EmploeeRole Role,
    string Email,
    Guid DepartmentId);
