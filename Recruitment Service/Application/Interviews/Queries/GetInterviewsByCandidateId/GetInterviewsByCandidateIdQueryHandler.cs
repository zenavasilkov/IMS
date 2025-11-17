using Application.Abstractions.Messaging;
using Application.Interviews.Queries.GetInterviewById;
using Domain.Contracts.Repositories;
using Domain.Shared;
using Mapster;
using Pagination;

namespace Application.Interviews.Queries.GetInterviewsByCandidateId;

public class GetInterviewsByCandidateIdQueryHandler(IInterviewRepository repository)
    : IQueryHandler<GetInterviewsByCandidateIdQuery, GetInterviewsByCandidateIdQueryResponse>
{
    public async Task<Result<GetInterviewsByCandidateIdQueryResponse>> Handle(
        GetInterviewsByCandidateIdQuery request, CancellationToken cancellationToken)
    {
        var interviews = await repository.GetByConditionAsync(i => i.CandidateId == request.CandidateId, request.PaginationParameters, false, cancellationToken);

        var pagedList = interviews.Adapt<PagedList<GetInterviewByIdQueryResponse>>();  

        var response = new GetInterviewsByCandidateIdQueryResponse(pagedList);

        return response;
    }
}
