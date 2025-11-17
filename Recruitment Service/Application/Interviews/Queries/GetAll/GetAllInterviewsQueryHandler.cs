using Application.Abstractions.Messaging;
using Application.Interviews.Queries.GetInterviewById;
using Domain.Contracts.Repositories;
using Domain.Shared;
using Mapster;
using Pagination;

namespace Application.Interviews.Queries.GetAll;

public class GetAllInterviewsQueryHandler(IInterviewRepository repository) : IQueryHandler<GetAllInterviewsQuery, GetAllInterviewsQueryResponse>
{
    public async Task<Result<GetAllInterviewsQueryResponse>> Handle(GetAllInterviewsQuery request, CancellationToken cancellationToken)
    {
        var interviews = await repository.GetByConditionAsync(i => true, request.PaginationParameters, false, cancellationToken);

        var pagedList = interviews.Adapt<PagedList<GetInterviewByIdQueryResponse>>();

        var response = new GetAllInterviewsQueryResponse(pagedList);

        return response;
    }
}
