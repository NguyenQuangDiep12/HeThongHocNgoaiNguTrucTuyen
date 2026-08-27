using HeThongHocNgoaiNguTrucTuyen.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeThongHocNgoaiNguTrucTuyen.Data.Configurations
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            // Table
            builder.ToTable("Lessons");

            // Primary Key
            builder.HasKey(l => l.LessonId);

            builder.Property(l => l.LessonId)
                .ValueGeneratedOnAdd();

            // TopicId
            builder.Property(l => l.TopicId)
                .IsRequired();

            // Title
            builder.Property(l => l.Title)
                .IsRequired()
                .HasMaxLength(200);

            // Description
            builder.Property(l => l.Description)
                .IsRequired(false)
                .HasMaxLength(500);

            // Content
            builder.Property(l => l.Content)
                .IsRequired(false)
                .HasColumnType("nvarchar(max)");

            // Lesson N - 1 Topic
            builder.HasOne(l => l.Topic)
                .WithMany(t => t.Lessons)
                .HasForeignKey(l => l.TopicId)
                .OnDelete(DeleteBehavior.Cascade);

            // Lesson 1 - N Vocabulary
            builder.HasMany(l => l.Vocabularies)
                .WithOne(v => v.Lesson)
                .HasForeignKey(v => v.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            // Lesson 1 - N LearningProgress
            builder.HasMany(l => l.LearningProgresses)
                .WithOne(lp => lp.Lesson)
                .HasForeignKey(lp => lp.LessonId)
                .OnDelete(DeleteBehavior.Restrict);
            // Ngan chan khi xoa Lesson khong xoa Tien trinh hoc tap cua nguoi dung
        }
    }
}