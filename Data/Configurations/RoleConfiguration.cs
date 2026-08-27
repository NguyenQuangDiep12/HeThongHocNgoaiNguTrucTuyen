using HeThongHocNgoaiNguTrucTuyen.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeThongHocNgoaiNguTrucTuyen.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // Table
            builder.ToTable("Roles");

            // Primary Key
            builder.HasKey(r => r.RoleId);

            builder.Property(r => r.RoleId)
                .ValueGeneratedOnAdd();

            // RoleName
            builder.Property(r => r.RoleName)
                .IsRequired()
                .HasMaxLength(50);

            // RoleName phải duy nhất
            builder.HasIndex(r => r.RoleName)
                .IsUnique();

            // Description
            builder.Property(r => r.Description)
                .IsRequired(false)
                .HasMaxLength(255);

            // Role 1 - N User
            builder.HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}