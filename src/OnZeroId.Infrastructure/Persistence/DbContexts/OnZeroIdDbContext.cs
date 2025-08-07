using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OnZeroId.Infrastructure.Persistence.Entities;

namespace OnZeroId.Infrastructure.Persistence.DbContexts;

public partial class OnZeroIdDbContext : DbContext
{
    public OnZeroIdDbContext(DbContextOptions<OnZeroIdDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<OauthAccounts> OauthAccounts { get; set; }

    public virtual DbSet<Passkeys> Passkeys { get; set; }

    public virtual DbSet<TotpKeys> TotpKeys { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("timescaledb")
            .HasPostgresExtension("timescaledb_toolkit");

        modelBuilder.Entity<OauthAccounts>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("oauth_accounts_pkey");

            entity.ToTable("oauth_accounts");

            entity.HasIndex(e => new { e.Provider, e.Sub }, "oauth_accounts_provider_sub_key").IsUnique();

            entity.HasIndex(e => new { e.UserId, e.Provider }, "oauth_accounts_user_id_provider_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Profile)
                .HasColumnType("jsonb")
                .HasColumnName("profile");
            entity.Property(e => e.Provider)
                .HasMaxLength(32)
                .HasColumnName("provider");
            entity.Property(e => e.Sub)
                .HasMaxLength(255)
                .HasColumnName("sub");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.OauthAccounts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("oauth_accounts_user_id_fkey");
        });

        modelBuilder.Entity<Passkeys>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("passkeys_pkey");

            entity.ToTable("passkeys");

            entity.HasIndex(e => e.UserId, "idx_passkeys_user_id");

            entity.HasIndex(e => e.CredentialId, "passkeys_credential_id_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Aaguid)
                .HasMaxLength(36)
                .HasColumnName("aaguid");
            entity.Property(e => e.BackedUp)
                .HasDefaultValue(false)
                .HasColumnName("backed_up");
            entity.Property(e => e.Counter)
                .HasDefaultValue(0)
                .HasColumnName("counter");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CredentialId).HasColumnName("credential_id");
            entity.Property(e => e.DeviceType)
                .HasMaxLength(32)
                .HasColumnName("device_type");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
            entity.Property(e => e.PublicKey).HasColumnName("public_key");
            entity.Property(e => e.Transports).HasColumnName("transports");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Passkeys)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("passkeys_user_id_fkey");
        });

        modelBuilder.Entity<TotpKeys>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("totp_keys_pkey");

            entity.ToTable("totp_keys");

            entity.HasIndex(e => e.UserId, "idx_totp_keys_user_id");

            entity.HasIndex(e => e.UserId, "totp_keys_user_id_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(false)
                .HasColumnName("is_active");
            entity.Property(e => e.Issuer)
                .HasMaxLength(64)
                .HasColumnName("issuer");
            entity.Property(e => e.Label)
                .HasMaxLength(128)
                .HasColumnName("label");
            entity.Property(e => e.LastUsedAt).HasColumnName("last_used_at");
            entity.Property(e => e.Secret).HasColumnName("secret");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.TotpKeys)
                .HasForeignKey<TotpKeys>(d => d.UserId)
                .HasConstraintName("totp_keys_user_id_fkey");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "idx_users_email");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .HasColumnName("email");
            entity.Property(e => e.EmailVerifiedAt).HasColumnName("email_verified_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LockedOutUntil).HasColumnName("locked_out_until");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.PasswordSetAt).HasColumnName("password_set_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(64)
                .HasColumnName("username");
        });
        modelBuilder.HasSequence("chunk_constraint_name", "_timescaledb_catalog");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
