using HeThongHocNgoaiNguTrucTuyen.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeThongHocNgoaiNguTrucTuyen.Data.Configurations
{
    public class LearningProgressConfiguration
        : IEntityTypeConfiguration<LearningProgress>
    {
        public void Configure(
            EntityTypeBuilder<LearningProgress> builder)
        {
            // Table
            builder.ToTable("LearningProgresses");

            // Primary Key
            builder.HasKey(lp => lp.ProgressId);

            builder.Property(lp => lp.ProgressId)
                .ValueGeneratedOnAdd();

            // UserId
            builder.Property(lp => lp.UserId)
                .IsRequired();

            // LessonId
            builder.Property(lp => lp.LessonId)
                .IsRequired();

            // Status
            builder.Property(lp => lp.Status)
                .IsRequired()
                .HasConversion<int>();

            // CompletionPercent
            builder.Property(lp => lp.CompletionPercent)
                .IsRequired()
                .HasPrecision(5, 2);

            // User 1 - N LearningProgress
            builder.HasOne(lp => lp.User)
                .WithMany(u => u.LearningProgresses)
                .HasForeignKey(lp => lp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Lesson 1 - N LearningProgress
            builder.HasOne(lp => lp.Lesson)
                .WithMany(l => l.LearningProgresses)
                .HasForeignKey(lp => lp.LessonId)
                .OnDelete(DeleteBehavior.Restrict);

            // Một User chỉ có một Progress cho một Lesson
            builder.HasIndex(lp => new
            {
                lp.UserId,
                lp.LessonId
            })
            .IsUnique();

            // CompletionPercent phải từ 0 đến 100
            builder.ToTable("LearningProgresses", table =>
            {
                table.HasCheckConstraint(
                    "CK_LearningProgresses_CompletionPercent",
                    "[CompletionPercent] >= 0 AND [CompletionPercent] <= 100"
                );
            });
        }
    }
}