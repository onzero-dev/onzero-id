using Minio.DataModel.Args;
using Minio;
using Microsoft.Extensions.Options;
using OnZeroId.Application.Interfaces;

namespace OnZeroId.Infrastructure.Services;

public class MinioService : IMinioService
{
    private readonly IMinioClient _client;
    private readonly string _bucket;

    public MinioService(IMinioClient client, IOptions<MinioOptions> options)
    {
        _client = client;
        _bucket = options.Value.Bucket;
    }

    public async Task<string> UploadQrAsync(byte[] data, string objectName, CancellationToken cancellationToken = default)
    {
        using var ms = new MemoryStream(data);
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_bucket)
            .WithObject(objectName)
            .WithStreamData(ms)
            .WithObjectSize(ms.Length)
            .WithContentType("image/png");
        await _client.PutObjectAsync(putObjectArgs, cancellationToken);
        return $"/{_bucket}/{objectName}";
    }

    public async Task DeleteObjectAsync(string objectName, CancellationToken cancellationToken = default)
    {
        var removeArgs = new RemoveObjectArgs()
            .WithBucket(_bucket)
            .WithObject(objectName);
        await _client.RemoveObjectAsync(removeArgs, cancellationToken);
    }

    public async Task<string> GetPresignedUrlAsync(string objectName, TimeSpan expiry, CancellationToken cancellationToken = default)
    {
        var args = new PresignedGetObjectArgs()
            .WithBucket(_bucket)
            .WithObject(objectName)
            .WithExpiry((int)expiry.TotalSeconds);
        return await _client.PresignedGetObjectAsync(args);
    }
}
