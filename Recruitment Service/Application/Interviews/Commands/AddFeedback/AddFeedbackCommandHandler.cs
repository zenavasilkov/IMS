using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Shared;
using static Application.Errors.ApplicationErrors;

namespace Application.Interviews.Commands.AddFeedback;

public class AddFeedbackCommandHandler(IInterviewRepository repository) : ICommandHandler<AddFeedbackCommand>
{
    public async Task<Result> Handle(AddFeedbackCommand request, CancellationToken cancellationToken)
    {
        var interview = await repository.GetByIdAsync(request.Id, true, cancellationToken);

        if (interview is null) return InterviewErrors.NotFound;

        var result = interview.AddFeedback(request.Feedback, request.IsPassed);

        if (result.IsFailure) return result.Error;

        await repository.UpdateAsync(interview, cancellationToken);

        return Result.Success();
    }
}
