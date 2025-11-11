using Application.Abstractions.Messaging;

namespace Application.Candidates.Commands.AcceptCandidateToInternship;

public sealed record AcceptCandidateToInternshipCommand(Guid Id) : ICommand;
