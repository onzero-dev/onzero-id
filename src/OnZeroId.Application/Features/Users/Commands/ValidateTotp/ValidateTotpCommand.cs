
using OnZeroId.Application.DTOs;

namespace OnZeroId.Application.Features.Users.Commands.ValidateTotp;

public class ValidateTotpCommand
{
    public ValidateTotpRequest Request { get; set; } = null!;
}
