using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Shared;
using Mapster;
using static Application.Errors.ApplicationErrors;

namespace Application.Interviews.Queries.GetInterviewById;

public class GetInterviewByIdQueryHandler(IInterviewRepository repository)
    : IQueryHandler<GetInterviewByIdQuery, GetInterviewByIdQueryResponse>
{
    public async Task<Result<GetInterviewByIdQueryResponse>> Handle(GetInterviewByIdQuery request, CancellationToken cancellationToken)
    {
        var interview = await repository.GetByIdAsync(request.Id, false, cancellationToken);

        if (interview is null) return InterviewErrors.NotFound;

        var response = interview.Adapt<GetInterviewByIdQueryResponse>();

        return response;
    }
}
