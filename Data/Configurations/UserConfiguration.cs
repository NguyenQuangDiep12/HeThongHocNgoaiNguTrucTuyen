using HeThongHocNgoaiNguTrucTuyen.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeThongHocNgoaiNguTrucTuyen.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Table
            builder.ToTable("Users");

            // Primary Key
            builder.HasKey(u => u.UserId);

            // UserId
            builder.Property(u => u.UserId)
                .ValueGeneratedOnAdd();

            // FullName
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);

            // Email
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            // PasswordHash
            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            // RoleId
            builder.Property(u => u.RoleId)
                .IsRequired();

            // User N - 1 Role
            builder.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // User 1 - N LearningProgress
            builder.HasMany(u => u.LearningProgresses)
                .WithOne(lp => lp.User)
                .HasForeignKey(lp => lp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User 1 - N TestResult
            builder.HasMany(u => u.TestResults)
                .WithOne(tr => tr.User)
                .HasForeignKey(tr => tr.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}