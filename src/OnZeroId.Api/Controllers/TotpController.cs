using Wolverine;
using Microsoft.AspNetCore.Mvc;
using OnZeroId.Application.DTOs;
using OnZeroId.Application.Features.Users.Commands.GenerateTotp;
using OnZeroId.Application.Features.Users.Commands.ValidateTotp;

namespace OnZeroId.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TotpController : ControllerBase
{
    private readonly IMessageBus _bus;
    public TotpController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpPost("generate")]
    public async Task<ActionResult<GenerateTotpResponse>> Generate([FromBody] GenerateTotpRequest request, CancellationToken cancellationToken)
    {
        var command = new GenerateTotpCommand { Request = request };
        var result = await _bus.InvokeAsync<GenerateTotpResponse>(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("validate")]
    public async Task<ActionResult<ValidateTotpResponse>> Validate([FromBody] ValidateTotpRequest request, CancellationToken cancellationToken)
    {
        var command = new ValidateTotpCommand { Request = request };
        var result = await _bus.InvokeAsync<ValidateTotpResponse>(command, cancellationToken);
        return Ok(result);
    }
}
