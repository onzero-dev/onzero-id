namespace OnZeroId.Application.Interfaces;

public interface IQrCodeService
{
    byte[] GenerateQrCodePng(string text);
}
