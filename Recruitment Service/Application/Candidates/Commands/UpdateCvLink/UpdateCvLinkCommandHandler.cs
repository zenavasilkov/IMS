using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Shared;
using static Application.Errors.ApplicationErrors;

namespace Application.Candidates.Commands.UpdateCvLink;

public class UpdateCvLinkCommandHandler(ICandidateRepository repository) : ICommandHandler<UpdateCvLinkCommand>
{
    public async Task<Result> Handle(UpdateCvLinkCommand request, CancellationToken cancellationToken)
    {
        var candidate = await repository.GetByIdAsync(request.Id, true, cancellationToken);

        if (candidate is null) return CandidateErrors.NotFound;

        var updateCvLinkResult = candidate.UpdateCvLink(request.NewCvLink);

        if (updateCvLinkResult.IsFailure) return updateCvLinkResult.Error;

        await repository.UpdateAsync(candidate, cancellationToken);

        return Result.Success();
    }
}
