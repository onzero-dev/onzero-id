
using OnZeroId.Domain.Interfaces.Repositories;
using OnZeroId.Domain.Entities;
using OnZeroId.Application.DTOs;
using AutoMapper;
using System.Security.Cryptography;
using Wolverine.Attributes;

namespace OnZeroId.Application.Features.Users.Commands.RegisterUser;

[WolverineHandler]
public class RegisterUserCommandHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto> HandleAsync(RegisterUserCommand command, CancellationToken cancellationToken)
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
        // 使用 PBKDF2 (HMACSHA256) 產生安全雜湊，格式: base64(salt):base64(hash)
        using var rng = RandomNumberGenerator.Create();
        byte[] salt = new byte[16];
        rng.GetBytes(salt);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);
        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }
}
