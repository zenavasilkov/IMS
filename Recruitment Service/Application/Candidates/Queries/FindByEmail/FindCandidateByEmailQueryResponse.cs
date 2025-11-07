namespace Application.Candidates.Queries.FindByEmail;

public sealed record FindCandidateByEmailQueryResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    bool IsApplied,
    string? PhoneNumber = null,
    string? CvLink = null,
    string? LinkedIn = null,
    string? Patronymic = null);
