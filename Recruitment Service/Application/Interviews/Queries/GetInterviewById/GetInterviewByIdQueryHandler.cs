using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Entities;
using Domain.Shared;
using static Application.Errors.ApplicationErrors;

namespace Application.Interviews.Queries.GetInterviewById;

public class GetInterviewByIdQueryHandler(
    IGenericReadOnlyRepository<Interview> repository)
    : IQueryHandler<GetInterviewByIdQuery, GetInterviewByIdQueryResponse>
{
    public async Task<Result<GetInterviewByIdQueryResponse>> Handle(GetInterviewByIdQuery request, CancellationToken cancellationToken)
    {
        var interview = await repository.GetByIdAsync(request.Id, false, cancellationToken);

        if (interview is null) return InterviewErrors.NotFound;

        var response = new GetInterviewByIdQueryResponse(
            request.Id,
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

        return response;
    }
}
