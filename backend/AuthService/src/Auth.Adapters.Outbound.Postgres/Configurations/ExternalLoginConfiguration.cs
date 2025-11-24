using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Auth.Core.Domain.Entities;

namespace Auth.Adapters.Outbound.Postgres.Configurations
{
    public class ExternalLoginConfiguration : IEntityTypeConfiguration<ExternalLogin>
    {
        public void Configure(EntityTypeBuilder<ExternalLogin> builder)
        {
            builder.ToTable("external_logins")
                .HasKey(el => new { el.UserId, el.Provider });

            builder.Property(el => el.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(el => el.Provider)
                .HasColumnName("provider")
                .HasConversion<string>()
                .IsRequired();

            builder.Property(el => el.ProviderUserId)
                .HasColumnName("provider_user_id")
                .IsRequired();
        }
    }
}
