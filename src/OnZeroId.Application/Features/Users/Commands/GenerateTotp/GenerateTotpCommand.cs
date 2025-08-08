
using OnZeroId.Application.DTOs;

namespace OnZeroId.Application.Features.Users.Commands.GenerateTotp;

public class GenerateTotpCommand
{
    public GenerateTotpRequest Request { get; set; } = null!;
}
