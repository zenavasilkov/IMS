using Application.Abstractions.Messaging;

namespace Application.Employees.Commands.MoveToDepartment;

public sealed record MoveToDepartmentCommand(Guid EmployeeId, Guid DepartmentId) : ICommand;
