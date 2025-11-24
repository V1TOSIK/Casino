using Auth.Core.Domain.Entities;
using Auth.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Auth.Adapters.Outbound.Postgres.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("users")
                .HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("id")
                .IsRequired();

            var emailConverter = new ValueConverter<Email?, string>(
                v => v == null ? null : v.Value,
                v => string.IsNullOrWhiteSpace(v) ? null : new Email(v)
            );

            var phoneNumberConverter = new ValueConverter<PhoneNumber?, string>(
                v => v == null ? null : v.Value,
                v => string.IsNullOrWhiteSpace(v) ? null : new PhoneNumber(v)
            );

            var passwordConverter = new ValueConverter<Password?, string>(
                v => v == null ? null : v.Value,
                v => string.IsNullOrWhiteSpace(v) ? null : new Password(v)
            );

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasConversion(emailConverter)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(u => u.PhoneNumber)
                .HasColumnName("phone_number")
                .HasConversion(phoneNumberConverter)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(u => u.Password)
                .HasColumnName("password")
                .HasConversion(passwordConverter)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(u => u.RoleId)
                .HasColumnName("role_id")
                .IsRequired();

            builder.Property(u => u.RegistrationDate)
                .HasColumnName("registration_date")
                .IsRequired();

            builder.Property(u => u.IsBanned)
                .HasColumnName("is_banned")
                .IsRequired();

            builder.Property(u => u.BanReason)
                .HasColumnName("ban_reason")
                .HasMaxLength(500);

            builder.Property(u => u.BannedAt)
                .HasColumnName("banned_at");

            builder.Property(u => u.IsDeleted)
                .HasColumnName("is_deleted")
                .IsRequired();

            builder.Property(u => u.DeletedAt)
                .HasColumnName("deleted_at");

            builder.HasMany(u => u.ExternalLogins)
                .WithOne()
                .HasForeignKey(el => el.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.PhoneNumber).IsUnique();
        }
    }
}
