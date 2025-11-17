using static Application.Errors.ApplicationErrors;
using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Shared;
using Mapster;

namespace Application.Candidates.Queries.FindById;

public class FindCandidateByIdQueryHandler(ICandidateRepository repository)
    : IQueryHandler<FindCandidateByIdQuery, FindCandidateByIdQueryResponse>
{
    public async Task<Result<FindCandidateByIdQueryResponse>> Handle(FindCandidateByIdQuery request, CancellationToken cancellationToken)
    {
        var candidate = await repository.GetByIdAsync(request.Id, false, cancellationToken);

        if (candidate is null) return CandidateErrors.NotFound;

        var response = candidate.Adapt<FindCandidateByIdQueryResponse>();

        return response;
    }
}
