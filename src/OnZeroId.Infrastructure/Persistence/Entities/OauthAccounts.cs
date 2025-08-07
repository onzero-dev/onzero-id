using System;
using System.Collections.Generic;

namespace OnZeroId.Infrastructure.Persistence.Entities;

public partial class OauthAccounts
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Provider { get; set; } = null!;

    public string Sub { get; set; } = null!;

    public string? Profile { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Users User { get; set; } = null!;
}
