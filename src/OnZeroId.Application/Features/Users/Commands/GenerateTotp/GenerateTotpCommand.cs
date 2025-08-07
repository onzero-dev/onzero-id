using MediatR;
using OnZeroId.Application.DTOs;

namespace OnZeroId.Application.Features.Users.Commands.GenerateTotp;

public class GenerateTotpCommand : IRequest<GenerateTotpResponse>
{
    public GenerateTotpRequest Request { get; set; } = null!;
}
