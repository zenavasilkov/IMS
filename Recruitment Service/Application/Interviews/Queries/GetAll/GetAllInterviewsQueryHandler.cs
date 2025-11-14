using Application.Abstractions.Messaging;
using Application.Interviews.Queries.GetInterviewById;
using Application.Interviews.Queries.GetInterviewsByCandidateId;
using Domain.Contracts.Repositories;
using Domain.Shared;
using Pagination;

namespace Application.Interviews.Queries.GetAll;

public class GetAllInterviewsQueryHandler(IInterviewRepository repository) : IQueryHandler<GetAllInterviewsQuery, GetAllInterviewsQueryResponse>
{
    public async Task<Result<GetAllInterviewsQueryResponse>> Handle(GetAllInterviewsQuery request, CancellationToken cancellationToken)
    {
        var interviews = await repository.GetByConditionAsync(i => true, request.PaginationParameters, false, cancellationToken);

        var items = interviews.Items.Select(GetInterviewsByCandidateIdQueryHandler.Map).ToList();

        var PagedItems = new PagedList<GetInterviewByIdQueryResponse>(items, interviews.PageNumber, interviews.PageSize, interviews.TotalCount);

        var response = new GetAllInterviewsQueryResponse(PagedItems);

        return response;
    }
}
