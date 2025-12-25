using Application.Abstractions.Messaging;
using Domain.Contracts.BlobStorage;
using Domain.Contracts.Repositories;
using Domain.Shared;
using static Application.Errors.ApplicationErrors;

namespace Application.Candidates.Commands.UpdateCv;

public class UpdateCvCommandHandler(
    ICandidateRepository repository,
    IBlobService blobService)
    : ICommandHandler<UpdateCvCommand>
{
    public async Task<Result> Handle(UpdateCvCommand request, CancellationToken cancellationToken)
    {
        var candidate = await repository.GetByIdAsync(request.CandidateId, true, cancellationToken);
        if (candidate is null) return CandidateErrors.NotFound;

        var extension = Path.GetExtension(request.File.FileName);
        var objectKey = $"cv_{candidate.Id}{extension}";
        
        await blobService.UploadAsync(request.File, objectKey,  cancellationToken);
        var updateResult = candidate.UpdateCvLink(objectKey);
        if (!updateResult.IsSuccess) return updateResult.Error;
        
        await repository.UpdateAsync(candidate, cancellationToken);

        return Result.Success();
    }
}
