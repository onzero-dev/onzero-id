using MediatR;
using OnZeroId.Application.DTOs;

namespace OnZeroId.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<UserDto>
{
    public RegisterUserRequest Request { get; set; } = null!;
}
