using Application.Abstractions.Messaging;
using Application.Errors;
using Domain.Contracts.BlobStorage;
using Domain.Contracts.Repositories;
using Domain.Errors;
using Domain.Shared;

namespace Application.Candidates.Queries.GetCandidateCvUrlById;

public sealed class GetCandidateCvUrlByIdQueryHandler(
    ICandidateRepository repository,
    IBlobService blobService)
    : IQueryHandler<GetCandidateCvUrlByIdQuery, string>
{
    public async Task<Result<string>> Handle(GetCandidateCvUrlByIdQuery request, CancellationToken cancellationToken)
    {
        var candidate = await repository.GetByIdAsync(request.CandidateId, false, cancellationToken);

        if (candidate is null)
        {
            return ApplicationErrors.CandidateErrors.NotFound;
        }

        if (string.IsNullOrWhiteSpace(candidate.CvLink))
        {
            return DomainErrors.CandidateErrors.EmptyCvLink;
        }
        
        var url = await blobService.GetPresignedUrlAsync(candidate.CvLink);

        return Result.Success(url);
    }
}
