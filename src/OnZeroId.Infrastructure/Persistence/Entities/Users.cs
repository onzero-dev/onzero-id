using System;
using System.Collections.Generic;

namespace OnZeroId.Infrastructure.Persistence.Entities;

public partial class Users
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string? Username { get; set; }

    public string? PasswordHash { get; set; }

    public DateTime? PasswordSetAt { get; set; }

    public DateTime? EmailVerifiedAt { get; set; }

    public DateTime? LockedOutUntil { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<OauthAccounts> OauthAccounts { get; set; } = new List<OauthAccounts>();

    public virtual ICollection<Passkeys> Passkeys { get; set; } = new List<Passkeys>();

    public virtual TotpKeys? TotpKeys { get; set; }
}
