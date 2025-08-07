
namespace OnZeroId.Domain.Entities;

public class TotpKey
{
    public Guid UserId { get; set; }
    public string Secret { get; set; } = null!;
    public bool IsValid { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Issuer { get; set; } = "OnZeroId";
}
