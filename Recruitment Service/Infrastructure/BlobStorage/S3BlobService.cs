using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Domain.Contracts.BlobStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.BlobStorage;

public class S3BlobService(IAmazonS3 s3Client, IOptions<BlobStorageOptions> options) : IBlobService
{
    private const int ExpirationTimeInMinutes = 5;
    
    public async Task<string> UploadAsync(IFormFile file, string fileName, CancellationToken cancellationToken = default)
    {
        using var newMemoryStream = new MemoryStream();
        await file.CopyToAsync(newMemoryStream, cancellationToken);
        newMemoryStream.Position = 0;

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = newMemoryStream,
            Key = fileName,
            BucketName = options.Value.BucketName,
            ContentType = file.ContentType
        };
        
        var fileTransferUtility = new TransferUtility(s3Client);
        await fileTransferUtility.UploadAsync(uploadRequest, cancellationToken);
        
        return fileName;
    }

    public Task<string> GetPresignedUrlAsync(string key)
    {
        if (string.IsNullOrEmpty(key)) return Task.FromResult(string.Empty);

        var request = new GetPreSignedUrlRequest
        {
            BucketName = options.Value.BucketName,
            Key = key,
            Expires = DateTime.UtcNow.AddMinutes(ExpirationTimeInMinutes),
            Verb = HttpVerb.GET,
            Protocol = Protocol.HTTP,
            ResponseHeaderOverrides = { ContentDisposition = "inline" }
        };

        var url = s3Client.GetPreSignedURL(request);
        return Task.FromResult(url);
    }
}
