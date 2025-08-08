using Wolverine;
using Microsoft.AspNetCore.Mvc;
using OnZeroId.Application.Features.Users.Commands.RegisterUser;
using OnZeroId.Application.DTOs;

namespace OnZeroId.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMessageBus _bus;

    public UsersController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand { Request = request };
        var result = await _bus.InvokeAsync<UserDto>(command, cancellationToken);
        return Ok(result);
    }
}
