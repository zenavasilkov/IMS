using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Shared;
using static Application.Errors.ApplicationErrors;

namespace Application.Candidates.Commands.ApplyCandidate;

public class ApplyCandidateCommandHandler(ICandidateRepository repository) : ICommandHandler<ApplyCandidateCommand>
{
    public async Task<Result> Handle(ApplyCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidate = await repository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

        if (candidate is null) return CandidateErrors.NotFound;

        var cadidateApplyResult = candidate.Apply();

        if (cadidateApplyResult.IsFailure) return cadidateApplyResult.Error;

        return Result.Success();
    }
}
