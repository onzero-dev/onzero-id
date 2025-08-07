using MediatR;
using OnZeroId.Domain.Interfaces.Repositories;
using OnZeroId.Domain.Entities;
using OnZeroId.Application.DTOs;
using AutoMapper;

namespace OnZeroId.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var existing = await _userRepository.GetByEmailAsync(command.Request.Email, cancellationToken).ConfigureAwait(false);
        if (existing != null)
            throw new Exception("Email already exists.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = command.Request.Email,
            Username = command.Request.Username,
            PasswordHash = HashPassword(command.Request.Password),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user, cancellationToken).ConfigureAwait(false);

        return _mapper.Map<UserDto>(user);
    }

    private string HashPassword(string password)
    {
        // TODO: 實作安全雜湊
        return password;
    }
}
