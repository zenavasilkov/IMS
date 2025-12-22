using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Shared;
using IMS.gRPC.Contracts.CreateUser;
using Mapster;
using static Application.Errors.ApplicationErrors;

namespace Application.Candidates.Commands.AcceptCandidateToInternship;

public class AcceptCandidateToInternshipCommandHandler(
    IUserService service,
    ICandidateRepository repository)
    : ICommandHandler<AcceptCandidateToInternshipCommand>
{
    public async Task<Result> Handle(AcceptCandidateToInternshipCommand request, CancellationToken cancellationToken)
    {
        var candidate = await repository.GetByIdAsync(request.Id, true, cancellationToken);

        if (candidate is null) return CandidateErrors.NotFound;

        var candidateApplyResult = candidate.AcceptCandidateToInternship();

        if (candidateApplyResult.IsFailure) return candidateApplyResult.Error;

        await repository.UpdateAsync(candidate, cancellationToken);

        var createUserRequest = candidate.Adapt<CreateUserRequest>();

        await service.CreateUser(createUserRequest);

        return Result.Success();
    }
}
