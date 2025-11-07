using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Entities;
using Domain.Shared;
using Pagination;
using static Application.Errors.ApplicationErrors;

namespace Application.Candidates.Commands.RegisterCandidate;

public class RegisterCandidateRequestHandler(ICandidateRepository repository) : ICommandHandler<RegisterCandidateCommand>
{
    public async Task<Result> Handle(RegisterCandidateCommand request, CancellationToken cancellationToken)
    {
        var paginationParameters = new PaginationParameters(1, 1);

        var candidate = await repository.GetByConditionAsync(c => c.Email == request.Email, paginationParameters, false, cancellationToken);

        if (candidate is not null && candidate.TotalCount > 0) return CandidateErrors.EmailIsNotUnique;

        var registerCandidateResult = Candidate.Create(
            Guid.NewGuid(),
            request.FirstName,
            request.LastName,
            request.Email,
            request.IsApplied,
            request.PhoneNumber,
            request.CvLink,
            request.LinkedIn,
            request.Patronymic);

        if (registerCandidateResult.IsFailure) return registerCandidateResult.Error;
        
        await repository.CreateAsync(registerCandidateResult.Value, cancellationToken);

        return Result.Success();
    }
}
