using static Application.Errors.ApplicationErrors;
using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Shared;
using Mapster;

namespace Application.Candidates.Queries.FindByEmail;

public class FindCandidateByEmailHandler(ICandidateRepository repository) : IQueryHandler<FindCandidateByEmailQuery, FindCandidateByEmailQueryResponse>
{
    public async Task<Result<FindCandidateByEmailQueryResponse>> Handle(FindCandidateByEmailQuery request, CancellationToken cancellationToken)
    {
        var candidate = await repository.GetByEmailAsync(request.Email, false, cancellationToken);

        if (candidate is null) return CandidateErrors.NotFound;

        var response = candidate.Adapt<FindCandidateByEmailQueryResponse>();

        return response;
    }
}
