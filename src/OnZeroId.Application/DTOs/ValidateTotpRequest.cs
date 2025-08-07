namespace OnZeroId.Application.DTOs;

public class ValidateTotpRequest
{
    public Guid UserId { get; set; }
    public string Code { get; set; } = null!;
}
