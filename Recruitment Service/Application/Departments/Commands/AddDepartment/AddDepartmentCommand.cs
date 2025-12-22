using Application.Abstractions.Messaging;

namespace Application.Departments.Commands.AddDepartment;

public sealed record AddDepartmentCommand(string Name, string? Description) : ICommand<Guid>;
