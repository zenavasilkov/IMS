namespace Infrastructure.BlobStorage;

public class BlobStorageOptions
{
    public const string SectionName = "BlobStorage";
    
    public string Endpoint { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
}
