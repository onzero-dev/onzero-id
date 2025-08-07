using System;
using System.Collections.Generic;

namespace OnZeroId.Infrastructure.Persistence.Entities;

public partial class Passkeys
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public byte[] CredentialId { get; set; } = null!;

    public byte[] PublicKey { get; set; } = null!;

    public string? Name { get; set; }

    public int Counter { get; set; }

    public string? Aaguid { get; set; }

    public string? DeviceType { get; set; }

    public List<string>? Transports { get; set; }

    public bool BackedUp { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Users User { get; set; } = null!;
}
