using Application.Abstractions.Messaging;
using Application.Interviews.Queries.GetInterviewById;
using Domain.Contracts.Repositories;
using Domain.Entities;
using Domain.Shared;
using Pagination;

namespace Application.Interviews.Queries.GetInterviewsByCandidateId;

public class GetInterviewsByCandidateIdQueryHandler(IInterviewRepository repository) : IQueryHandler<GetInterviewsByCandidateIdQuery, GetInterviewsByCandidateIdQueryResponse>
{
    public async Task<Result<GetInterviewsByCandidateIdQueryResponse>> Handle(GetInterviewsByCandidateIdQuery request, CancellationToken cancellationToken)
    {
        var interviews = await repository.GetByConditionAsync(i => i.CandidateId == request.CandidateId, request.PaginationParameters, false, cancellationToken);

        var items = interviews.Items.Select(Map).ToList();

        var PagedItems = new PagedList<GetInterviewByIdQueryResponse>(items, interviews.PageNumber, interviews.PageSize, interviews.TotalCount);

        var response = new GetInterviewsByCandidateIdQueryResponse(PagedItems);

        return response;
    }

    internal static GetInterviewByIdQueryResponse Map(Interview interview) => new(
                interview.Id,
                interview.CandidateId,
                interview.InterviewerId,
                interview.DepartmentId,
                interview.Candidate!.Email,
                interview.Interviewer!.Email,
                interview.Department!.Name,
                interview.Type,
                interview.ScheduledAt,
                interview.Feedback,
                interview.IsPassed,
                interview.IsCancelled);
}
