using OnZeroId.Application.DTOs;
using OnZeroId.Application.Interfaces;
using OnZeroId.Domain.Entities;
using OnZeroId.Domain.Interfaces.Repositories;
using OtpNet;
using Wolverine.Attributes;

namespace OnZeroId.Application.Features.Users.Commands.GenerateTotp;

[WolverineHandler]
public class GenerateTotpCommandHandler
{
    private readonly IMinioService _minioService;
    private readonly IQrCodeService _qrCodeService;
    private readonly ITotpKeyRepository _totpKeyRepository;

    public GenerateTotpCommandHandler(
        IMinioService minioService,
        IQrCodeService qrCodeService,
        ITotpKeyRepository totpKeyRepository)
    {
        _minioService = minioService;
        _qrCodeService = qrCodeService;
        _totpKeyRepository = totpKeyRepository;
    }

    public async Task<GenerateTotpResponse> HandleAsync(GenerateTotpCommand command, CancellationToken cancellationToken)
    {
        // 1. 產生 TOTP Secret (Base32)
        var secretBytes = KeyGeneration.GenerateRandomKey(20);
        var secretBase32 = Base32Encoding.ToString(secretBytes);


        // 2. 若已有 TOTP 記錄則更新 secret 與 update time，否則新增
        var existing = await _totpKeyRepository.GetByUserIdAsync(command.Request.UserId, cancellationToken).ConfigureAwait(false);
        TotpKey totpRecord;
        if (existing != null)
        {
            existing.Secret = secretBase32;
            existing.IsValid = false;
            existing.UpdatedAt = DateTime.UtcNow;
            await _totpKeyRepository.UpdateAsync(existing, cancellationToken).ConfigureAwait(false);
            totpRecord = existing;
        }
        else
        {
            totpRecord = new TotpKey
            {
                UserId = command.Request.UserId,
                Secret = secretBase32,
                IsValid = false,
                UpdatedAt = DateTime.UtcNow
            };
            await _totpKeyRepository.AddAsync(totpRecord, cancellationToken).ConfigureAwait(false);
        }

        // 3. 產生 otpauth URL (符合 Otp.NET 建議)
        var label = command.Request.UserId.ToString();
        var otpauth = new OtpUri(
            OtpType.Totp,
            totpRecord.Secret,
            label,
            totpRecord.Issuer
        ).ToString();

        // 4. 產生 QRCode 圖片
        var qrBytes = _qrCodeService.GenerateQrCodePng(otpauth);
        var objectName = $"totp/{command.Request.UserId}.png";
        await _minioService.UploadQrAsync(qrBytes, objectName, cancellationToken);

        // 5. 產生 15 分鐘有效的下載連結
        var qrDownloadUrl = await _minioService.GetPresignedUrlAsync(objectName, TimeSpan.FromMinutes(15), cancellationToken);

        // 6. 回傳
        return new GenerateTotpResponse
        {
            QrCodeUrl = qrDownloadUrl
        };
    }
}
