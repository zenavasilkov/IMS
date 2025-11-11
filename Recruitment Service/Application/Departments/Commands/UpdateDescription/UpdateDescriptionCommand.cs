using Application.Abstractions.Messaging;

namespace Application.Departments.Commands.UpdateDescription;

public sealed record UpdateDescriptionCommand(Guid Id, string? NewDescription) : ICommand;
