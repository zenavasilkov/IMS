using Application.Abstractions.Messaging;
using Domain.Enums;

namespace Application.Employees.Commands.ChangeRole;

public sealed record ChangeRoleCommand(Guid EmployeeId, EmploeeRole NewRole) : ICommand;
