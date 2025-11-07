namespace Application.Candidates.Queries.FindById;

public sealed record FindCandidateByIdQueryResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    bool IsApplied,
    string? PhoneNumber = null,
    string? CvLink = null,
    string? LinkedIn = null,
    string? Patronymic = null);
