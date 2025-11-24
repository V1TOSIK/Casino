using Auth.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Adapters.Outbound.Postgres.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.ToTable("roles")
                .HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(r => r.Name)
                .HasColumnName("name")
                .IsRequired();

            builder.HasMany(r => r.Permissions)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "role_permissions",
                    j => j
                        .HasOne<PermissionEntity>()
                        .WithMany()
                        .HasForeignKey("permission_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<RoleEntity>()
                        .WithMany()
                        .HasForeignKey("role_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.ToTable("role_permissions");
                        j.HasKey("role_id", "permission_id");
                    });
        }
    }
}
