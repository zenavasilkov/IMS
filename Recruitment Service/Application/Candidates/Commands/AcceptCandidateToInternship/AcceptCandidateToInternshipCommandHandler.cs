using Application.Abstractions.Messaging;
using Application.Grpc;
using Domain.Contracts.Repositories;
using Domain.Shared;
using Mapster;
using static Application.Errors.ApplicationErrors;

namespace Application.Candidates.Commands.AcceptCandidateToInternship;

public class AcceptCandidateToInternshipCommandHandler(UserGrpcService.UserGrpcServiceClient userClient,
    ICandidateRepository repository) : ICommandHandler<AcceptCandidateToInternshipCommand>
{
    public async Task<Result> Handle(AcceptCandidateToInternshipCommand request, CancellationToken cancellationToken)
    {
        var candidate = await repository.GetByIdAsync(request.Id, true, cancellationToken);

        if (candidate is null) return CandidateErrors.NotFound;

        var cadidateApplyResult = candidate.AcceptCandidateToInternship();

        if (cadidateApplyResult.IsFailure) return cadidateApplyResult.Error;

        await repository.UpdateAsync(candidate, cancellationToken);

        var createUserRequest = candidate.Adapt<CreateUserGrpcRequest>();

        await userClient.CreateAsync(createUserRequest, cancellationToken: cancellationToken);

        return Result.Success();
    }
}
