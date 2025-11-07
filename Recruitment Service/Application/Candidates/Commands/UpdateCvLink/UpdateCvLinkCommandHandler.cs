using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Shared;
using static Application.Errors.ApplicationErrors;

namespace Application.Candidates.Commands.UpdateCvLink;

public class UpdateCvLinkCommandHandler(ICandidateRepository repository) : ICommandHandler<UpdateCvLinkCommand>
{
    public async Task<Result> Handle(UpdateCvLinkCommand request, CancellationToken cancellationToken)
    {
        var candidate = await repository.GetByIdAsync(request.Id, cancellationToken : cancellationToken);

        if (candidate is null) return CandidateErrors.NotFound;

        var updateCvLinkREsult = candidate.UpdateCvLink(request.NewCvLink);

        if (updateCvLinkREsult.IsFailure) return updateCvLinkREsult.Error;

        return Result.Success();
    }
}
