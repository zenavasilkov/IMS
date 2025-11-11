using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Entities;
using Domain.Shared;
using static Application.Errors.ApplicationErrors;

namespace Application.Interviews.Commands.RescheduleInterview;

public class RescheduleInterviewCommandHandler(IGenericRepository<Interview> repository) : ICommandHandler<RescheduleInterviewCommand>
{
    public async Task<Result> Handle(RescheduleInterviewCommand request, CancellationToken cancellationToken)
    {
        var interview = await repository.GetByIdAsync(request.Id, true, cancellationToken);

        if (interview is null) return InterviewErrors.NotFound;

        var result = interview.Reschedule(request.NewDate);

        if (result.IsFailure) return result.Error;

        await repository.UpdateAsync(interview, cancellationToken);

        return Result.Success();
    }
}
