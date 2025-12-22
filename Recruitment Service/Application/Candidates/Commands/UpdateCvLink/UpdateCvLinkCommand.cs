using Application.Abstractions.Messaging;

namespace Application.Candidates.Commands.UpdateCvLink;

public sealed record UpdateCvLinkCommand(Guid Id, string? NewCvLink) : ICommand;
