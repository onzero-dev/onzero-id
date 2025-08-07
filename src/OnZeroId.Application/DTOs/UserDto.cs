namespace OnZeroId.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string? Username { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
