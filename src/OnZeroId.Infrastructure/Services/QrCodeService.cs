using OnZeroId.Application.Interfaces;
using QRCoder;

namespace OnZeroId.Infrastructure.Services;

public class QrCodeService : IQrCodeService
{
    public byte[] GenerateQrCodePng(string text)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCode = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        using var bitmap = new PngByteQRCode(qrCode);
        return bitmap.GetGraphic(20);
    }
}
