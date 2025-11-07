using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Entities;
using Domain.Shared;
using Domain.ValueObjects;
using static Application.Errors.ApplicationErrors;

namespace Application.Candidates.Commands.RegisterCandidate;

public class RegisterCandidateRequestHandler(ICandidateRepository repository) : ICommandHandler<RegisterCandidateCommand>
{
    public async Task<Result> Handle(RegisterCandidateCommand request, CancellationToken cancellationToken)
    { 
        var candidate = await repository.GetByEmailAsync(request.Email, false, cancellationToken);

        if (candidate is not null) return CandidateErrors.EmailIsNotUnique;

        var fullName = FullName.Create(request.FirstName, request.LastName, request.Patronymic);

        if (fullName.IsFailure) return fullName.Error;

        var registerCandidateResult = Candidate.Create(
            Guid.NewGuid(),
            fullName.Value,
            request.Email,
            request.PhoneNumber,
            request.CvLink,
            request.LinkedIn);

        if (registerCandidateResult.IsFailure) return registerCandidateResult.Error;
        
        await repository.CreateAsync(registerCandidateResult.Value, cancellationToken);

        return Result.Success();
    }
}
