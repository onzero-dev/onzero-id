namespace OnZeroId.Application.DTOs;

public class RegisterUserRequest
{
    public string Email { get; set; } = null!;
    public string? Username { get; set; }
    public string Password { get; set; } = null!;
}
