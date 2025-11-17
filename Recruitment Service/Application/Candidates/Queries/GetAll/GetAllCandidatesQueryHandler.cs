using Application.Abstractions.Messaging;
using Application.Candidates.Queries.FindById;
using Domain.Contracts.Repositories;
using Domain.Shared;
using Mapster;
using Pagination;

namespace Application.Candidates.Queries.GetAll;

public class GetAllCandidatesQueryHandler(ICandidateRepository repository) : IQueryHandler<GetAllCandidatesQuery, GetAllCandidatesQueryResponce>
{
    public async Task<Result<GetAllCandidatesQueryResponce>> Handle(GetAllCandidatesQuery request, CancellationToken cancellationToken)
    {
        var candidates = await repository.GetByConditionAsync(c => true, request.PaginationParameters, false, cancellationToken);

        var pagedList = candidates.Adapt<PagedList<FindCandidateByIdQueryResponse>>();

        var response = new GetAllCandidatesQueryResponce(pagedList);

        return response;
    }
}
