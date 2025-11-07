using Application.Abstractions.Messaging;

namespace Application.Candidates.Commands.RegisterCandidate;

public sealed record RegisterCandidateCommand(
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber = null,
    string? CvLink = null,
    string? LinkedIn = null,
    string? Patronymic = null) : ICommand;
