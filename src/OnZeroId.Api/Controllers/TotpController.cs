using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnZeroId.Application.DTOs;
using OnZeroId.Application.Features.Users.Commands.GenerateTotp;
using OnZeroId.Application.Features.Users.Commands.ValidateTotp;

namespace OnZeroId.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TotpController : ControllerBase
{
    private readonly IMediator _mediator;
    public TotpController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("generate")]
    public async Task<ActionResult<GenerateTotpResponse>> Generate([FromBody] GenerateTotpRequest request, CancellationToken cancellationToken)
    {
        var command = new GenerateTotpCommand { Request = request };
        var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost("validate")]
    public async Task<ActionResult<ValidateTotpResponse>> Validate([FromBody] ValidateTotpRequest request, CancellationToken cancellationToken)
    {
        var command = new ValidateTotpCommand { Request = request };
        var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }
}
