using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Shared;
using static Application.Errors.ApplicationErrors;

namespace Application.Candidates.Commands.AcceptCandidateToInternship;

public class AcceptCandidateToInternshipCommandHandler(ICandidateRepository repository) : ICommandHandler<AcceptCandidateToInternshipCommand>
{
    public async Task<Result> Handle(AcceptCandidateToInternshipCommand request, CancellationToken cancellationToken)
    {
        var candidate = await repository.GetByIdAsync(request.Id, true, cancellationToken);

        if (candidate is null) return CandidateErrors.NotFound;

        var cadidateApplyResult = candidate.AcceptCandidateToInternship();

        if (cadidateApplyResult.IsFailure) return cadidateApplyResult.Error;

        await repository.UpdateAsync(candidate, cancellationToken);

        return Result.Success();
    }
}
