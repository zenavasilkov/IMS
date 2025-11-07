using static Application.Errors.ApplicationErrors;
using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Shared;

namespace Application.Candidates.Queries.FindByEmail;

public class FindCandidateByEmailHandler(ICandidateRepository repository) : IQueryHandler<FindCandidateByEmailQuery, FindCandidateByIdQueryResponse>
{
    public async Task<Result<FindCandidateByIdQueryResponse>> Handle(FindCandidateByEmailQuery request, CancellationToken cancellationToken)
    {
        var candidate = await repository.GetByEmailAsync(request.Email, false, cancellationToken);

        if (candidate is null) return CandidateErrors.NotFound;

        var response = new FindCandidateByIdQueryResponse(
            candidate.Id,
            candidate.FirstName,
            candidate.LastName,
            candidate.Email,
            candidate.IsApplied,
            candidate.PhoneNumber,
            candidate.CvLink,
            candidate.LinkedIn,
            candidate.Patronymic);

        return response;
    }
}
