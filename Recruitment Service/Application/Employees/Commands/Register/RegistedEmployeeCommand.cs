using Application.Abstractions.Messaging;
using Domain.Enums;

namespace Application.Employees.Commands.Register;

public sealed record RegistedEmployeeCommand(
    string FirstName,
    string LastName, 
    EmploeeRole Role,
    string Email,
    Guid DepartmentId,
    string? Patronymic = null) : ICommand;
