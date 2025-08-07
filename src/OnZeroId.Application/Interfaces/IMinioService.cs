using System.Threading;
using System.Threading.Tasks;

namespace OnZeroId.Application.Interfaces;

public interface IMinioService
{
    Task<string> UploadQrAsync(byte[] data, string objectName, CancellationToken cancellationToken = default);
    Task DeleteObjectAsync(string objectName, CancellationToken cancellationToken = default);
    Task<string> GetPresignedUrlAsync(string objectName, TimeSpan expiry, CancellationToken cancellationToken = default);
}
