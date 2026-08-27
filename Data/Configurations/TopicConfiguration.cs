using HeThongHocNgoaiNguTrucTuyen.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeThongHocNgoaiNguTrucTuyen.Data.Configurations
{
    public class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            // Table
            builder.ToTable("Topics");

            // Primary Key
            builder.HasKey(t => t.TopicId);

            builder.Property(t => t.TopicId)
                .ValueGeneratedOnAdd();

            // LanguageId
            builder.Property(t => t.LanguageId)
                .IsRequired();

            // Name
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(150);

            // Level
            builder.Property(t => t.Level)
                .IsRequired(false)
                .HasMaxLength(50);

            // Description
            builder.Property(t => t.Description)
                .IsRequired(false)
                .HasMaxLength(500);

            // ImageUrl
            builder.Property(t => t.ImageUrl)
                .IsRequired(false)
                .HasMaxLength(500);

            // Topic N - 1 Language
            builder.HasOne(t => t.Language)
                .WithMany(l => l.Topics)
                .HasForeignKey(t => t.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);

            // Topic 1 - N Lesson
            builder.HasMany(t => t.Lessons)
                .WithOne(l => l.Topic)
                .HasForeignKey(l => l.TopicId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}