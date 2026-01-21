using Microsoft.AspNetCore.Http;

namespace Domain.Contracts.BlobStorage;

public interface IBlobService
{
    Task<string> UploadAsync(IFormFile  file, string fileName, CancellationToken cancellationToken = default);
    Task<string> GetPresignedUrlAsync(string key);
}
