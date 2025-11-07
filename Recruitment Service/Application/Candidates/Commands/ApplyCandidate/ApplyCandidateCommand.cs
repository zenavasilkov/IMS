using Application.Abstractions.Messaging;

namespace Application.Candidates.Commands.ApplyCandidate;

public sealed record ApplyCandidateCommand(Guid Id) : ICommand;
