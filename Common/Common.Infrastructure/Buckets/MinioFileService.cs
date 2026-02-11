using Common.Application.Buckets;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Common.Infrastructure.Buckets;

public class MinioFileService(
    IMinioClient minio,
    IOptions<MinioSettings> options) : IFileService
{
    private readonly IMinioClient _minio = minio;
    private readonly MinioSettings _minioSettings = options.Value;

    public string ResolveUrl(string filePath)
    {
        return $"http://{_minioSettings.PublicEndpoint}/{_minioSettings.Bucket}/{filePath}";
    }


    public Task<string> UploadFileAsync(IFormFile file)
    {
        string accessLevel = true ? "public" : "private";
        return UploadFileAsync(file, $"{file.ContentType.Split('/')[0]}/{accessLevel}");
    }
    private async Task<string> UploadFileAsync(IFormFile file, string prefix)
    {
        string objectName = $"{prefix}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        using var stream = file.OpenReadStream();

        await _minio.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_minioSettings.Bucket)
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(file.ContentType));

        return objectName;
    }

}