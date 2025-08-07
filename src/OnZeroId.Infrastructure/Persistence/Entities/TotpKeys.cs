using System;
using System.Collections.Generic;

namespace OnZeroId.Infrastructure.Persistence.Entities;

public partial class TotpKeys
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Secret { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? Issuer { get; set; }

    public string? Label { get; set; }

    public DateTime? LastUsedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Users User { get; set; } = null!;
}
