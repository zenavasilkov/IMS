using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Shared;
using static Application.Errors.ApplicationErrors;

namespace Application.Interviews.Commands.CancelInterview;

public class CancelInterviewCommandHandler(IInterviewRepository repository) : ICommandHandler<CancelInterviewCommand>
{
    public async Task<Result> Handle(CancelInterviewCommand request, CancellationToken cancellationToken)
    {
        var interview = await repository.GetByIdAsync(request.Id, true, cancellationToken);

        if (interview is null) return InterviewErrors.NotFound;

        var result = interview.Cancel();

        if (result.IsFailure) return result.Error;

        await repository.UpdateAsync(interview, cancellationToken);

        return Result.Success();
    }
}
