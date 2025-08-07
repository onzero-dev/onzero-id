
using MediatR;
using OnZeroId.Application.DTOs;
using OnZeroId.Domain.Interfaces.Repositories;
using OnZeroId.Application.Interfaces;
using OtpNet;

namespace OnZeroId.Application.Features.Users.Commands.ValidateTotp;

public class ValidateTotpCommandHandler : IRequestHandler<ValidateTotpCommand, ValidateTotpResponse>
{
    private readonly ITotpKeyRepository _totpKeyRepository;
    private readonly IMinioService _minioService;
    public ValidateTotpCommandHandler(ITotpKeyRepository totpKeyRepository, IMinioService minioService)
    {
        _totpKeyRepository = totpKeyRepository;
        _minioService = minioService;
    }

    public async Task<ValidateTotpResponse> Handle(ValidateTotpCommand command, CancellationToken cancellationToken)
    {
        var key = await _totpKeyRepository.GetByUserIdAsync(command.Request.UserId, cancellationToken).ConfigureAwait(false);
        if (key == null)
            return new ValidateTotpResponse { Success = false, Message = "TOTP not registered." };

        // 使用 Otp.NET 驗證 TOTP
        var totp = new Totp(Base32Encoding.ToBytes(key.Secret));
        bool isValid = totp.VerifyTotp(command.Request.Code, out long _, VerificationWindow.RfcSpecifiedNetworkDelay);

        if (isValid)
        {
            // 首次驗證成功才設 valid，並刪除 Minio 圖片
            if (!key.IsValid)
            {
                await _totpKeyRepository.SetValidAsync(command.Request.UserId, cancellationToken).ConfigureAwait(false);
                var objectName = $"totp/{command.Request.UserId}.png";
                await _minioService.DeleteObjectAsync(objectName, cancellationToken).ConfigureAwait(false);
            }
            return new ValidateTotpResponse { Success = true };
        }
        return new ValidateTotpResponse { Success = false, Message = "Invalid code." };
    }
}
