using Application.Abstractions.Messaging;
using Domain.Enums;

namespace Application.Employees.Commands.Register;

public sealed record RegisterEmployeeCommand(
    string FirstName,
    string LastName, 
    EmploeeRole Role,
    string Email,
    Guid DepartmentId,
    string? Patronymic = null) : ICommand;
