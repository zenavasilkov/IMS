using Application.Abstractions.Messaging;

namespace Application.Departments.Commands.RenameDepartment;

public sealed record RenameDepartmentCommand(Guid Id, string NewName) : ICommand;
